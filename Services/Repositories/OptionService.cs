using erp_server.Data;
using erp_server.Dtos;
using erp_server.Models;
using erp_server.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services.Repositories
{
    public class OptionService(AppDbContext context) : BaseService<Option>(context)
    {
        private new readonly AppDbContext _context = context;

        public async Task<Option> CreateOption(
            string name,
            double price = 0,
            OptionGroupType type = 0,
            List<string>? children = null,
            bool require = false,
            Guid? optionDepends = null // ✅ 新增參數
        )
        {
            var newOption = new Option
            {
                Id = Guid.NewGuid(),
                Name = name,
                Price = price,
                Type = type,
                Require = require,
                // ✅ 設定 OptionDepends 的資料，如果有的話
                Depend = optionDepends
            };

            _context.Options.Add(newOption);

            // 當 type 為 Single，處理 children
            if (type == OptionGroupType.Single && children != null && children.Count > 0)
            {
                List<Guid> optionChildrenIds = [];
                foreach (var childName in children)
                {
                    var newOptionChild = new OptionChildren
                    {
                        Id = Guid.NewGuid(),
                        Name = childName
                    };

                    _context.OptionChildren.Add(newOptionChild);
                    optionChildrenIds.Add(newOptionChild.Id);
                }

                foreach (var childrenId in optionChildrenIds)
                {
                    var optionRadio = new OptionRadio
                    {
                        OptionId = newOption.Id,
                        ChildrenId = childrenId
                    };

                    _context.OptionRadios.Add(optionRadio);
                }
            }

            await _context.SaveChangesAsync();
            return newOption;
        }
        public async Task<List<OptionResponse>> GetAllOptionsAsync()
        {
            var options = await _context.Options.ToListAsync();
            var optionChildren = await _context.OptionChildren.ToListAsync();
            var optionRadios = await _context.OptionRadios.ToListAsync();

            // 查找 materials 表格的所有資料
            var materials = await _context.Materials.ToListAsync();

            // 取得 OptionDepends，這裡我們查的是 materials 的資料
            var dependMaterials = options
                .Where(o => o.Depend != null)
                .Select(o => o.Depend.Value)
                .Distinct()
                .ToList();

            var dependOptions = await _context.Materials
                .Where(m => dependMaterials.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => new OptionDependDto
                {
                    Id = m.Id,
                    Name = m.Name
                });

            var result = new List<OptionResponse>();

            foreach (var option in options)
            {
                var optionResponse = new OptionResponse
                {
                    Id = option.Id,
                    Name = option.Name,
                    Price = option.Price,
                    Type = (OptionGroupType)option.Type,
                    Require = (option.Type == OptionGroupType.Single) ? option.Require : (bool?)null,
                    OptionDepends = option.Depend != null && dependOptions.TryGetValue(option.Depend.Value, out var dependDto)
                        ? dependDto
                        : null
                };

                if (option.Type == OptionGroupType.Single) // Single 選項才有 Children
                {
                    var childIds = optionRadios
                        .Where(r => r.OptionId == option.Id)
                        .Select(r => r.ChildrenId)
                        .ToList();

                    optionResponse.Children = optionChildren
                        .Where(c => childIds.Contains(c.Id))
                        .Select(c => new OptionChildDto
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToList();
                }

                result.Add(optionResponse);
            }

            return result;
        }

        public bool Drop(Guid id)
        {
            var option = _context.Options.FirstOrDefault(o => o.Id == id);
            if (option == null)
                return false;

            // 刪除相關的 OptionRadios
            var relatedRadios = _context.OptionRadios.Where(r => r.OptionId == id).ToList();
            if (relatedRadios.Count != 0)
                _context.OptionRadios.RemoveRange(relatedRadios);

            _context.Options.Remove(option);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<OptionResponse>> GetOptionsByIdsAsync(Guid[] optionIds)
        {
            var options = await _context.Options
                .Where(o => optionIds.Contains(o.Id))
                .ToListAsync();

            var optionChildren = await _context.OptionChildren.ToListAsync();
            var optionRadios = await _context.OptionRadios.ToListAsync();
            var materials = await _context.Materials.ToListAsync();

            // 取得 OptionDepends (依賴的 materials)
            var dependMaterials = options
                .Where(o => o.Depend != null)
                .Select(o => o.Depend.Value)
                .Distinct()
                .ToList();

            var dependOptions = await _context.Materials
                .Where(m => dependMaterials.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => new OptionDependDto
                {
                    Id = m.Id,
                    Name = m.Name
                });

            var result = new List<OptionResponse>();

            foreach (var option in options)
            {
                var optionResponse = new OptionResponse
                {
                    Id = option.Id,
                    Name = option.Name,
                    Price = option.Price,
                    Type = (OptionGroupType)option.Type,
                    Require = (option.Type == OptionGroupType.Single) ? option.Require : (bool?)null,
                    OptionDepends = option.Depend != null && dependOptions.TryGetValue(option.Depend.Value, out var dependDto)
                        ? dependDto
                        : null
                };

                if (option.Type == OptionGroupType.Single)
                {
                    var childIds = optionRadios
                        .Where(r => r.OptionId == option.Id)
                        .Select(r => r.ChildrenId)
                        .ToList();

                    optionResponse.Children = [.. optionChildren
                        .Where(c => childIds.Contains(c.Id))
                        .Select(c => new OptionChildDto
                        {
                            Id = c.Id,
                            Name = c.Name
                        })];
                }

                result.Add(optionResponse);
            }

            return result;
        }


    }
}
