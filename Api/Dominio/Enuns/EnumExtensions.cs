namespace MinimalApi.Dominio.Enuns
{
    public static class EnumExtensions
    {
        public static TEnum ParseEnum<TEnum>(string value) where TEnum : Enum
        {
            try
            {
                return (TEnum)Enum.Parse(typeof(TEnum), value);
            }
            catch (Exception){
                return default(TEnum);
            }            
        }
    }
}

