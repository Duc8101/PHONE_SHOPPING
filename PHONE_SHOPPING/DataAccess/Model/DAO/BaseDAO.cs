namespace DataAccess.Model.DAO
{
    public class BaseDAO
    {
        internal readonly PHONE_SHOPPINGContext _context;
        public BaseDAO(PHONE_SHOPPINGContext context)
        {
            _context = context;
        }

    }
}
