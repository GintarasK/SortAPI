using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Services.Sort
{
    public class SortService : ISortService
    {
        public List<int> BubbleMethod(List<int> list)
        {
            if (list?.Any() != true)
            {
                throw new ValidationException($"Invalid parameter {nameof(list)} passed");
            }

            for (var j = list.Count() - 1; j > 0; j--)
            {
                for (var i = 0; i < j; i++)
                {
                    if (list[i] > list[i + 1])
                    {
                        int temp;

                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                    }
                }
            }

            return list;
        }
    }
}