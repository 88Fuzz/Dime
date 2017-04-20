using System.Collections.Generic;
using System.Text;

/*
 * This is a fucking horse shit of a class. For some unknown stupid ass reason, the OnGUI Event.character does not concatenate with "" nor with StringBuilder
 * So I have to build my own string class, which is fucking gross.
 * 
 * I should do more of a look into why that ass clown Event object doesn't concatenate like you'd expect.
 */
public class MyString
{
    private static readonly int DEFAULT_SIZE = 0;

    private List<char> characters;
    private int indexCount;

    public MyString()
    {
        characters = new List<char>(DEFAULT_SIZE);
        indexCount = 0;
    }

    public void Add(char character)
    {
        if (indexCount >= characters.Count)
            characters.Add(character);
        else
            characters[indexCount] = character;
        indexCount++;
    }

    public void RemoveLast()
    {
        if (--indexCount < 0)
            indexCount = 0;
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder(indexCount);

        for(int i = 0; i < indexCount; i++)
        {
            builder.Append(characters[i]);
        }

        return builder.ToString();
    }
    
    public void Clear()
    {
        indexCount = 0;
    }
}