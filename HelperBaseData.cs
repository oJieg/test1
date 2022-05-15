
namespace test1
{
    public static class HelperBaseData
    {
        public static bool ValidationOffset(int offset, int amoutOfContact)
        {
            if (offset < 0 || offset > amoutOfContact)
            {
                return false;
            }
            return true;
        }
    }
}
