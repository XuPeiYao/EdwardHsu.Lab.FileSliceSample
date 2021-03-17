using System;
using System.Linq;

using Xunit;

namespace EdwardHsu.Lab.FileSliceSample.Tests
{
    public class StreamSlicerTest
    {
        [Fact]
        public void Test1()
        {
            using var file = System.IO.File.Open(
                @"C:\Users\XuPeiYao\Downloads\book.png",
                System.IO.FileMode.Open);

            using var slicer = new StreamSlicer(file);

            foreach (var item in slicer)
            {
                Console.WriteLine(item.Length);
            }

            Assert.Equal(slicer.Count(), Math.Ceiling(file.Length / 4096M));
        }
    }
}
