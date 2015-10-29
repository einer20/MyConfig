using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyConfig.Standars
{
    internal sealed class PropertyStandars
    {

        

        [Obsolete("Usar el metodo sobre cargado, esto no sirve :D")]
        public static bool IsValidPropertyOrThrow(string propertyInfo)
        {

            if (string.IsNullOrEmpty(propertyInfo))
            {
                throw new ArgumentNullException("propertyInfo");
            }
            else if (propertyInfo.Contains(' '))
            {
                throw new FormatException("Invalid format '" + propertyInfo + "'. Cannot exist spaces between the property name and the asigned value");
            }
            else if (propertyInfo.Contains('=') == false)
            {
                throw new FormatException("Invalid format '" + propertyInfo + "'. Must set the = sign to asign a value to a property");
            }
            else
            {
                return true;
            }
        }

        [Obsolete("No usar")]
        public static bool IsValidProperty(string propertyInfo)
        {
            return Parser.CommandExpression.IsMatch(propertyInfo);
        }
    }

    

}
