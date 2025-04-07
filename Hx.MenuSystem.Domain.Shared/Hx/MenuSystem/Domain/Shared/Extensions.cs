namespace Hx.MenuSystem.Domain.Shared
{
    public static class Extensions
    {
        public static SubjectType ToSubjectType(this string providerName)
        {
            if (providerName == "U")
            {
                return SubjectType.User;
            }
            else if (providerName == "R")
            {
                return SubjectType.Role;
            }
            else if (providerName == "O")
            {
                return SubjectType.Organization;
            }
            return SubjectType.User;
        }
    }
}
