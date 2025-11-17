namespace QuizMaster.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var type = typeof(T);
            var memberInfo = type.GetMember(enumValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumValue.ToString();
        }
    }
}
