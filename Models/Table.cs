using RestaurantApp.Localization;

namespace RestaurantApp.Models;

/// <summary>
/// Стіл у ресторані.
/// </summary>
public class Table
{
    public int Number { get; set; }
    public int Seats { get; set; }
    public bool IsOccupied { get; set; }
    public Order? CurrentOrder { get; set; }

    public Table(int number, int seats)
    {
        Number = number;
        Seats = seats;
    }

    public string StatusLabel => Loc.Get(IsOccupied ? "Table_Occupied" : "Table_Free");

    public override string ToString() => Loc.Get("Table_Label", Number, Seats, StatusLabel);
}
