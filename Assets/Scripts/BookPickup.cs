using UnityEngine;

public class BookPickup : MonoBehaviour
{
    [Header("Book ID")]
    public string bookId = "book_ginjal"; // PENTING: Buat unique untuk setiap book!
    
    public int amount = 1;

    [Header("Popup Text")]
    [TextArea] public string message1 = "Ginjal.\n\nDia adalah sebuah organ untuk menetralisir racun yang masuk kedalam tubuh!";
    [TextArea] public string message2Prefix = "Organ ini dapat rusak jika kita sering mengonsumsi asupan yang bersifat junk food. Total organ terkumpul: ";

    PlayerPopupController popup;
    bool taken;

    void Start()
    {
        popup = FindObjectOfType<PlayerPopupController>(true);
        
        // Jika book sudah pernah diklaim, hancurkan
        if (BookManager.Instance != null && BookManager.Instance.IsBookClaimed(bookId))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (taken) return;

        if (other.CompareTag("Player"))
        {
            taken = true;

            BookManager.Instance.AddBook(amount, bookId);

            if (popup != null)
            {
                popup.ShowSequence(
                    message1,
                    message2Prefix + BookManager.Instance.totalBooks
                );
            }

            Destroy(gameObject);
        }
    }
}