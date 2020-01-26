using Moq;
using Services.Sort;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Tests
{
    public class SortTest
    {
        private readonly List<int> unsortedList = new List<int> { 5, 2, 8, 10, 1, };
        private readonly List<int> sortedList = new List<int> { 1, 2, 5, 8, 10, };

        [Fact]
        public void ValidateNullParameter()
        {
            var sortService = new SortService();
            Assert.Throws<ValidationException>(() => sortService.BubbleMethod(null));
        }

        [Fact]
        public void ValidateEmptyListPrameter()
        {
            var sortService = new SortService();
            Assert.Throws<ValidationException>(() => sortService.BubbleMethod(new List<int>()));
        }

        [Fact]
        public void ValidateSort()
        {
            var sortService = new SortService();
            Assert.Equal(sortedList, sortService.BubbleMethod(unsortedList));
        }

        [Fact]
        public void ValidateSortReturnsSameNumbers()
        {
            var sortService = new SortService();
            var randomList = GenerateRandomList(100);
            Assert.True(sortService.BubbleMethod(randomList).Intersect(randomList).Any());
        }

        [Fact]
        public void ValidateSortReturnsSameLengthList()
        {
            var sortService = new SortService();
            var randomList = GenerateRandomList(100);
            Assert.True(sortService.BubbleMethod(randomList)?.Count == randomList.Count);
        }

        [Fact]
        public void ValidateSortNonConsecutiveList()
        {
            var sortService = new SortService();
            var randomList = GenerateRandomList(1000, 100);
            Assert.True(IsListOrdered(sortService.BubbleMethod(randomList)));
        }

        private bool IsListOrdered(List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i-1] > list[i])
                {
                    return false;
                }
            }

            return true;
        }

        private List<int> GenerateRandomList(int maxValue, int? length = null)
        {
            length = length != null ? length : maxValue;
            var random = new Random();
            var randomlist = new List<int>();

            for (int i = 0; i < length; i++)
            {
                randomlist.Add(random.Next(maxValue));
            }

            return randomlist;
        }
    }
}
