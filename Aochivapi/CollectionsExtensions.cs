namespace Aochivapi;

public static class CollectionsExtensions
{
    public static List<string> SplitAndSave(this string str, params string[] separators)
    {
        var list = new List<string>();
        var openings = str.Split(separators[0]);
        for (var i = 0; i < openings.Length; i++)
        {
            var closings = openings[i].Split(separators[1]);
            for (var j = 0; j < closings.Length; j++)
            {
                list.Add(closings[j]);
                if (j != closings.Length - 1)
                    list.Add(separators[1]);
            }

            if (i != openings.Length - 1)
                list.Add(separators[0]);
        }

        return list;
    }
}