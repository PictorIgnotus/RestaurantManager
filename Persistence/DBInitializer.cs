using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;


namespace Persistence
{
    public static class DBInitializer
    {
        public static void Initialize(IApplicationBuilder app)
        {
            var ServiceScope = app.ApplicationServices.CreateScope();
            RestaurantContext context = ServiceScope.ServiceProvider.GetService<RestaurantContext>();
            context.Database.EnsureCreated();

            // database already initialized
            if (context.Products.Any())
            {
                return;
            }

            IList<Category> categoryList = new List<Category>
            {
                new Category
                {
                    Type = CategoryType.Soup,
                    Name = "Levesek",
                },
                new Category
                {
                    Type = CategoryType.Hamburger,
                    Name = "Hamburgerek",
                },
                new Category
                {
                    Type = CategoryType.Sandwich,
                    Name = "Sandwichek",
                },
                new Category
                {
                    Type = CategoryType.Pizza,
                    Name = "Pizzák",
                },
                new Category
                {
                    Type = CategoryType.Coffee,
                    Name = "Kávék",
                },
                new Category
                {
                    Type = CategoryType.SoftDrink,
                    Name = "Üdítők",
                }
            };

            foreach (Category cat in categoryList)
                context.Categories.Add(cat);

            context.SaveChanges();

            IList<Product> productsList = new List<Product>
            {
                new Product("Marhahúsleves velőscsonttal", CategoryType.Soup, 2300)
                {
                    Description = "Gazdagon zöldségekkel, eperlevéllel, tormával, mustárral, pirítóssal, Erős Pistával",
                    SaleNumber = 10,
                    Hot = true,
                    Vegetarian = false,
                },
                new Product("Buffalo's classic",CategoryType.Soup, 3100)
                {
                    Description = "Fûszeres sajtleves alap, csirkemellcsíkok, marinált paprika, jalapeno, tortilla",
                    SaleNumber = 4,
                    Hot = true,
                    Vegetarian = false,
                },
                new Product("Chedar sajtos leves",CategoryType.Soup, 1700)
                {
                    Description = "Snidlinges krutonnal",
                    SaleNumber = 13,
                    Hot = false,
                    Vegetarian = true
                },
                new Product("Hideg eperkrémleves",CategoryType.Soup, 1400)
                {
                    Description = "Marcipánforgáccsal",
                    SaleNumber = 4,
                    Hot = false,
                    Vegetarian = true,
                },
                new Product("Ausztrál óriás hamburger",CategoryType.Hamburger, 3000)
                {
                    Description = "40 dkg szaftos húspogácsa, házi hamburger szósz, friss zöldség, cheddar sajt + 1 db választható feltét, dorrál chips, amerikai káposztasaláta",
                    SaleNumber = 8,
                    Hot = false,
                    Vegetarian = false,
                },
                new Product("Wellington burger", CategoryType.Hamburger, 3290)
                {
                    Description = "40 dkg szaftos marhahús pogácsa, érlelt sonka, bacon, gomba, cheddar, monterey jack, kéksajt, magos mustár, vajas tésztában sütve, kéksajtos jégsalátával",
                    SaleNumber = 5,
                    Hot = false,
                    Vegetarian = false
                },
                new Product("Csirke Burger",CategoryType.Hamburger, 2390)
                {
                    Description = "2x15 dkg friss fûszernövényekkel ízesített húspogácsa, cibatta zsemlékbe töltve, friss zöldségekkel, mozzarella sajttal, amerikai káposztasalátával",
                    SaleNumber = 10,
                    Hot = false,
                    Vegetarian = false,
                },
                new Product("Óriás buffalo steak sandwich",CategoryType.Sandwich, 3990)
                {
                    Description = "50 dkg hússal megtöltött óriás szendvics, jalapeno paprikával és cheddar sajttal sütve, amerikai káposztasalátával",
                    SaleNumber = 7,
                    Hot = true,
                    Vegetarian = false,
                },
                new Product("Crostino Napoletano",CategoryType.Pizza, 1190)
                {
                    Description = "Kemencében sült fokhagymás pizza kenyér, paradicsomszósz, mozzarella, cheddar, maasdamer, szárított paradicsom",
                    SaleNumber = 13,
                    Hot = false,
                    Vegetarian = true,
                },
                new Product("Gondola Pollo e Gorgonzola",CategoryType.Pizza, 1700)
                {
                    Description = "Kemencében sült pizza kenyér paradicsomszósszal, mozzarellával, csirkemellel és gorgonzolával töltve, fokhagymás vajjal megkenve, rucolával és bruschetta mixszel tálalva",
                    SaleNumber = 7,
                    Hot = false,
                    Vegetarian = false,
                },
                new Product("Pizza Gusti di Primavera",CategoryType.Pizza, 2490)
                {
                    Description = "Spenót, mediterrán lágy sajt, párolt paprika mix, szárított paradicsom, fekete olívabogyó, fokhagymás olaj, mozzarella, paradicsomszósz",
                    SaleNumber = 18,
                    Hot = false,
                    Vegetarian = true,
                },
                new Product("Három sajtos sandwich",CategoryType.Sandwich, 8500)
                {
                    Description = "trappista, camembert, márványsajt, fejessaláta, paradicsom",
                    SaleNumber = 30,
                    Hot = false,
                    Vegetarian = true,
                },
                new Product("Latte",CategoryType.Coffee, 590)
                {
                    SaleNumber = 20,
                },
                new Product("Jeges kávé",CategoryType.Coffee, 780)
                {
                    SaleNumber = 10,
                },
                new Product("Ír kávé",CategoryType.Coffee, 890)
                {
                    SaleNumber = 6,
                },
                new Product("Dr. Pepper",CategoryType.SoftDrink, 700)
                {
                    SaleNumber = 10,
                },
                new Product("Coca Cola",CategoryType.SoftDrink, 830)
                {
                    SaleNumber = 10,
                },
                new Product("Sprite",CategoryType.SoftDrink, 580)
                {
                    SaleNumber = 25,
                },
            };


            foreach (Product product in productsList)
                context.Products.Add(product);

            context.SaveChanges();
        }
    }
}