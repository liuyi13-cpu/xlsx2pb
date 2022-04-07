using System.ComponentModel;
using System.Linq;

public enum EMacroDefine
{
    [Description("Debug")]
    ENABLE_DEBUG,
}
public static class EnumExtension
{
    public static string ToDescription(this EMacroDefine val)
    {
        var type = val.GetType();
        var memberInfo = type.GetMember(val.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes == null || attributes.Length != 1) {
            //如果没有定义描述，就把当前枚举值的对应名称返回
            return val.ToString();
        }
        return (attributes.Single() as DescriptionAttribute).Description;
    }
}