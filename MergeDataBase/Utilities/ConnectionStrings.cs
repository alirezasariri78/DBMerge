namespace DBDiff.Models
{
    internal class ConnectionStrings
    {
        public static string GetConnectionString(ServerInstance serverInstance, DataBaseInstance dataBaseInstance = null)
        {
            string result = $"Provider=sqloledb; Data Source={serverInstance.Name};";

            if (dataBaseInstance != null)
                result += $"Initial Catalog = {dataBaseInstance.Name};";

            if (!serverInstance.Authentication.UseWindowsAuthentication)
                result += "Persist Security Info=True;"; 

            if (!serverInstance.Authentication.UseWindowsAuthentication)
                result += $" User ID = {serverInstance.Authentication.UserName}; Password ={serverInstance.Authentication.Password};";

            return result;
        }
    }
}
