namespace erp_server
{
    public enum Role
    {
        Admin,
        Customer
    }
    public class JwtUserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Role Role { get; set; }  // 建議這裡改成 enum 類型
    }

}
