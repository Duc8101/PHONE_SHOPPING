using DataAccess.Entity;

namespace DataAccess.Model.DAO
{
    public class BaseDAO
    {
        protected readonly PHONE_SHOPPINGContext context = new PHONE_SHOPPINGContext();
    }
}
