using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace PizzaOrder.Business.Helpers
{
    public static class CursorHelper
    {
        public static string ToCursor(int Id) => Convert.ToBase64String(BitConverter.GetBytes(Id));
        public static int FromCursor(string base64String) => BitConverter.ToInt32(Convert.FromBase64String(base64String),0);

        public static (string firstCursor,string lastCursor) GetFirstAndLastCursor(List<int> enumerable)
        {
            if (!enumerable.Any())
            {
                return (null, null);
            }

            var firstCursor = ToCursor(enumerable.First());
            var lastCursor = ToCursor(enumerable.Last());

            return (firstCursor, lastCursor);
        }
    }
}