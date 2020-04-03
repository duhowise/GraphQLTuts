using System;

namespace PizzaOrder.Data
{
    [Flags]
    public enum Toppings
    {
        None=1,
        Pepperoni=2,
        Mushrooms=3,
        Onions=4,
        Sausage=5,
        Bacon=6,
        ExtraCheese=7,
        BlackOlives=8
    }
}