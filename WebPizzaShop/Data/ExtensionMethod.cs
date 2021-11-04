using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;


namespace WebPizzaShop.Data
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// Перевод копеек(int) в рубли (string)
        /// </summary>
        /// <param name="cop"></param>
        /// <returns></returns>
        public static string IntToRub(this int cop)
        {
            double r =  Convert.ToDouble(cop) / 100;
            return r.ToString("n2", CultureInfo.GetCultureInfo("ru-Ru"));
        }

    }
}
