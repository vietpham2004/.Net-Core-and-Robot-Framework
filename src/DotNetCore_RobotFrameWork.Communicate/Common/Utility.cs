using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetCore_RobotFrameWork.Communicate.Common
{
    public static class Utility
    {
        public const string FormatNumberWithCurrency = "{0:C2}";
        public const string FormatNumberNoDigits = "{0:n0}";
        public const string FormatNumber2Digits = "{0:n2}";
        public const string FormatDate = "{0:MM/dd/yyyy}";
        public const string FormatTareNumber = "#,0.0000";

        /// <summary>
        /// The value in <column_name> on index=<index> is incorrect <actual result> != <expected_result>
        /// </summary>
        public const string Error_Message_Verify_Failed_In_Grid = "The value in {0} on index={1} is incorrect {2} != {3}";
        /// <summary>
        /// <Label name> is incorrect <actual result> != Expected result
        /// </summary>
        public const string Error_Message_Verify_Failed_Lable = "{0} is incorrect {1} != {2}";

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }


        public static bool CheckExistEnum<T>(string value)
        {
            return Enum.IsDefined(typeof(TestDataType), value);
        }

        public static bool CompareText(this string compareValue, string value, ref string valueOut, FormatType type = FormatType.Text)
        {
            double tmp = 0;
            if (type == FormatType.Currency || type == FormatType.Numeric || type == FormatType.NumericNoDigit)
            {
                tmp = double.Parse(compareValue, NumberStyles.Currency);
                if (type == FormatType.Currency)
                    compareValue = string.Format(FormatNumberWithCurrency, tmp);//string.Format(FormatNumberWithCurrency, tmp > 0 ? tmp : Math.Abs(tmp));
                else if (type == FormatType.Numeric)
                {
                    compareValue = string.Format(FormatNumber2Digits, tmp > 0 ? tmp : Math.Abs(tmp));
                    if (tmp < 0)
                        compareValue = string.Format("({0})", compareValue);
                }
                else if (type == FormatType.NumericNoDigit)
                {
                    compareValue = string.Format(FormatNumberNoDigits, tmp > 0 ? tmp : Math.Abs(tmp));
                    if (tmp < 0)
                        compareValue = string.Format("({0})", compareValue);
                }
            }
            else if (type == FormatType.Text)
                compareValue = Regex.Replace(compareValue, @"\t|\n|\r", "");

            valueOut = compareValue;
            return compareValue.Equals(value);
        }

        public static bool EqualsText(this string actualText, string expectedText, bool ignoreCase = true)
        {
            if (ignoreCase)
                return actualText.Equals(expectedText, StringComparison.CurrentCultureIgnoreCase);

            return actualText.Equals(expectedText);
        }

        public static bool NotEqualsText(this string actualText, string expectedText, bool ignoreCase = true)
        {
            if (ignoreCase)
                return !actualText.Equals(expectedText, StringComparison.CurrentCultureIgnoreCase);

            return !actualText.Equals(expectedText);
        }

        public static bool ContainsText(this string actualText, string expectedText, bool ignoreCase = true)
        {
            if (ignoreCase)
                return actualText.Contains(expectedText, StringComparison.CurrentCultureIgnoreCase);

            return actualText.Contains(expectedText);
        }

        public static bool NotContainsText(this string actualText, string expectedText, bool ignoreCase = false)
        {
            if (ignoreCase)
                return !actualText.Contains(expectedText, StringComparison.CurrentCultureIgnoreCase);

            return !actualText.Contains(expectedText);
        }

        public static string GetNameByValue<T>(int value)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray().GetValue(value).ToString();
        }

        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.GetName());
            var descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return descriptionAttribute == null ? value.GetName() : descriptionAttribute.Description;
        }

		

        public static string GetLocatorWithoutStrategy(this string locator)
        {
            string[] arr = locator.Split('=');
            if (arr.Length == 2)
                return arr[1];
            return locator;
        }

        public static string CurrentCultureName()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        public static string FormatCurrency(string amount)
        {
            var actualValue = double.Parse(amount, NumberStyles.Currency);
            return string.Format(FormatNumberWithCurrency, actualValue);
        }

        public static string GetShortDateFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }

        public static string GetLongTimeFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
        }

        public static string GetShortTimeFormat()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        }
        public static double ToDouble(this string value)
        {
            return double.Parse(value, NumberStyles.Currency);
        }
        public static double ToDouble(this object value)
        {
            return double.Parse(value.ToString(), NumberStyles.Currency);
        }
        public static bool ToBolean(this object value)
        {
            return bool.Parse(value.ToString());
        }
        public static int ToInt(this string value)
        {
            return int.Parse(value, NumberStyles.Currency);
        }
        public static int ToInt(this object value)
        {
            return int.Parse(value.ToString(), NumberStyles.Currency);
        }
    }
}
