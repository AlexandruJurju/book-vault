namespace BookShop.Catalog.Domain.Categories;

public static class CategoryErrors
{
    public static string NotFound(Guid categoryId) => $"The category with the identifier {categoryId} was not found";
}
