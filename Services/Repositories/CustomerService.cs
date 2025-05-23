using erp_server.Data;
using erp_server.Dtos;
using erp_server.Models;
using erp_server.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services.Repositories
{
    public class CustomerService(AppDbContext context) : BaseService<Customer>(context)
    {
        private new readonly AppDbContext _context = context;

        public async Task<Customer?> GetByLineUserIdAsync(string lineUserId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.LineUserId == lineUserId);
        }

        public async Task<Customer> CreateCustomerAsync(string lineUserId, string displayName, string? pictureUrl, string accessToken)
        {
            var customer = new Customer
            {
                LineUserId = lineUserId,
                DisplayName = displayName,
                PictureUrl = pictureUrl,
                AccessToken = accessToken
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateLastLoginAsync(Customer customer)
        {
            customer.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

    }

}
