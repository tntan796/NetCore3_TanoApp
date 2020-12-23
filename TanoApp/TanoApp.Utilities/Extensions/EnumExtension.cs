using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace TanoApp.Utilities.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T e) where T: IConvertible
        {
            string description = null;
            if (e is Enum) {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);
                foreach(int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribtes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttribtes.Length > 0)
                        {
                            // we're only getting the first description we find
                            description = ((DescriptionAttribute)descriptionAttribtes[0]).Description;
                        }
                        break;
                    }
                }
            }
            return description;
        }
    }
}
