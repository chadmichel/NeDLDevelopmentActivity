using Microsoft.EntityFrameworkCore;

public class ShoppingDatabase : DbContext
{

    public ShoppingDatabase(DbContextOptions<ShoppingDatabase> options) : base(options)
    {

    }

    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }
}