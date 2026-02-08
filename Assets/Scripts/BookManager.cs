using UnityEngine;
using System.Collections.Generic;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance;

    public int totalBooks;
    
    // Track book yang sudah diklaim (per ID)
    private HashSet<string> claimedBooks = new HashSet<string>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadClaimedBooks();
    }

    public void AddBook(int amount, string bookId)
    {
        if (claimedBooks.Contains(bookId))
            return; // Sudah diklaim

        totalBooks += amount;
        claimedBooks.Add(bookId);
        SaveClaimedBooks();
    }

    public bool IsBookClaimed(string bookId)
    {
        return claimedBooks.Contains(bookId);
    }

    private void SaveClaimedBooks()
    {
        // Simpan ke PlayerPrefs
        PlayerPrefs.SetInt("TotalBooks", totalBooks);
        
        string booksStr = string.Join(",", claimedBooks);
        PlayerPrefs.SetString("ClaimedBooks", booksStr);
        PlayerPrefs.Save();
    }

    private void LoadClaimedBooks()
    {
        totalBooks = PlayerPrefs.GetInt("TotalBooks", 0);
        
        string booksStr = PlayerPrefs.GetString("ClaimedBooks", "");
        if (!string.IsNullOrEmpty(booksStr))
        {
            string[] bookArray = booksStr.Split(',');
            foreach (string bookId in bookArray)
            {
                if (!string.IsNullOrEmpty(bookId))
                    claimedBooks.Add(bookId);
            }
        }
    }

    public void ResetAllBooks()
    {
        totalBooks = 0;
        claimedBooks.Clear();
        PlayerPrefs.DeleteKey("TotalBooks");
        PlayerPrefs.DeleteKey("ClaimedBooks");
        PlayerPrefs.Save();
    }
}