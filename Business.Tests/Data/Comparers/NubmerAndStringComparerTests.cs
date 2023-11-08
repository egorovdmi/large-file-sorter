using Business.Data.Comparers;

namespace Business.Tests.Data.Comparers;

[TestFixture]
public class NubmerAndStringComparerTests
{
    [Test]
    public void Compare_WithDifferentStrings_ShouldReturnCorrectOrder()
    {
        var comparer = new NubmerAndStringComparer();

        int result = comparer.Compare("1. Apple", "2. Banana");
        Assert.Less(result, 0); // Expected "1. Apple" to be less than "2. Banana"

        result = comparer.Compare("3. Orange", "1. Apple");
        Assert.Greater(result, 0); // Expected "3. Orange" to be greater than "1. Apple"

        result = comparer.Compare("1. Pineapple", "4. Apple");
        Assert.Greater(result, 0); // Expected "1. Pineapple" to be greater than "4. Apple"

        result = comparer.Compare("5. Orange", "6. Orange");
        Assert.Less(result, 0); // Expected "5. Orange" to be less than "6. Orange"
    }

    [Test]
    public void Compare_WithEqualStrings_ShouldReturnZero()
    {
        var comparer = new NubmerAndStringComparer();

        int result = comparer.Compare("5. Orange", "5. Orange");
        Assert.That(result, Is.EqualTo(0)); // Expected "5. Orange" to be equal to "5. Orange"
    }

    [Test]
    public void Sort_ListOfStrings_ShouldBeSortedCorrectly()
    {
        var comparer = new NubmerAndStringComparer();

        var list = new List<string> { "415. Apple", "30432. Something something something", "1. Apple", "32. Cherry is the best", "2. Banana is yellow" };

        list.Sort(comparer);

        Assert.That(list, Is.EqualTo(new List<string> { "1. Apple", "415. Apple", "2. Banana is yellow", "32. Cherry is the best", "30432. Something something something" }));
    }
}
