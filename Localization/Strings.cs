using System.Collections.Generic;

namespace RestaurantApp.Localization;

/// <summary>
/// Усі тексти інтерфейсу для обох мов. Ключі в Uk та En мають збігатися.
/// Тексти з {0}, {1} — шаблони для string.Format.
/// </summary>
internal static class Strings
{
    public static readonly Dictionary<string, string> Uk = new()
    {
        // Шапка та вкладки
        ["Header_Subtitle"]   = "Система управління рестораном",
        ["Stat_ActiveOrders"] = "Активних замовлень",
        ["Stat_Dishes"]       = "Страв у меню",
        ["Stat_Waiters"]      = "Офіціантів",
        ["Stat_Revenue"]      = "Виручка сьогодні",
        ["Tab_Orders"]        = "📝 Замовлення",
        ["Tab_Menu"]          = "🍕 Меню",
        ["Tab_Waiters"]       = "👥 Офіціанти",

        // Статуси замовлення
        ["Status_Pending"]    = "⏳ Очікує",
        ["Status_InProgress"] = "👨‍🍳 Готується",
        ["Status_Served"]     = "✅ Подано",
        ["Status_Paid"]       = "💳 Сплачено",
        ["Status_Cancelled"]  = "❌ Скасовано",

        // Столи, страви
        ["Table_Free"]        = "🟢 Вільний",
        ["Table_Occupied"]    = "🔴 Зайнятий",
        ["Table_Label"]       = "Стіл №{0} ({1} місць) — {2}",
        ["Dish_Available"]    = "✅ В наявності",
        ["Dish_Unavailable"]  = "❌ Немає",

        // Помилки та підсумки
        ["Err_Discount"]      = "Знижка має бути від 0 до 100.",
        ["Err_OrderClosed"]   = "Не можна додавати страви до закритого замовлення.",
        ["Order_Summary"]     = "Замовлення #{0} | Стіл {1} | {2} | {3:F2} ₴",

        // Вкладка «Замовлення»
        ["Orders_Title"]      = "Замовлення",
        ["Orders_New"]        = "Нове замовлення",
        ["Orders_Table"]      = "Стіл",
        ["Orders_Waiter"]     = "Офіціант",
        ["Orders_Create"]     = "➕ Створити замовлення",
        ["Order_Number"]      = "Замовлення #{0}",
        ["Table_Number"]      = "🪑 Стіл №{0}",
        ["Orders_Details"]    = "Деталі замовлення",
        ["Orders_Add"]        = "Додати",
        ["Orders_Discount"]   = "Знижка (%):",
        ["Orders_ToPay"]      = "До сплати:",
        ["Orders_Actions"]    = "Дії",
        ["Orders_StatusChange"] = "Зміна статусу",
        ["Orders_ToKitchen"]  = "👨‍🍳  Передати на кухню",
        ["Orders_Served"]     = "✅  Замовлення подано",
        ["Orders_Payment"]    = "Оплата",
        ["Orders_Pay"]        = "💳  Прийняти оплату",
        ["Orders_Cancel"]     = "❌  Скасувати замовлення",
        ["Orders_InfoTitle"]  = "ℹ️ Інформація",
        ["Orders_InfoText"]   = "Для оплати замовлення воно повинно мати статус 'Подано'. Знижку можна задати у центральній панелі.",

        // Вкладка «Меню»
        ["Filter_All"]        = "Всі",
        ["Menu_Title"]        = "Меню",
        ["Menu_AddTitle"]     = "Додати страву",
        ["Menu_Emoji"]        = "Емодзі",
        ["Menu_Name"]         = "Назва *",
        ["Menu_Description"]  = "Опис",
        ["Menu_Price"]        = "Ціна (₴) *",
        ["Menu_Category"]     = "Категорія *",
        ["Menu_AddButton"]    = "➕ Додати до меню",
        ["Menu_RemoveButton"] = "🗑  Видалити вибрану",
        ["Menu_ClearButton"]  = "Очистити форму",
        ["Menu_Hint"]         = "* — обов'язкові поля. Для редагування натисніть на картку страви — поля заповняться автоматично.",

        // Вкладка «Офіціанти»
        ["Waiters_Title"]       = "Офіціанти",
        ["Waiters_ActiveCount"] = "{0} активних",
        ["Waiters_Orders"]      = "Замовлення офіціанта",
        ["Waiters_Completed"]   = "Виконано",
        ["Waiters_Revenue"]     = "Виручка",
        ["Waiters_NewTitle"]    = "Новий офіціант",
        ["Waiters_Avatar"]      = "Аватар",
        ["Waiters_Name"]        = "Ім'я *",
        ["Waiters_Phone"]       = "Телефон",
        ["Waiters_Add"]         = "➕ Додати офіціанта",
        ["Waiters_Remove"]      = "🗑  Видалити обраного",
        ["Waiters_Clear"]       = "Очистити",

        // Повідомлення про дії
        ["Msg_OrderCreated"]  = "✅ Замовлення #{0} створено для столу {1}",
        ["Msg_DishAdded"]     = "➕ Додано: {0} × {1}",
        ["Msg_DishRemoved"]   = "🗑 Видалено: {0}",
        ["Msg_Status"]        = "Статус: {0}",
        ["Msg_Paid"]          = "💳 Сплачено {0:F2} ₴. Дякуємо!",
        ["Msg_Cancelled"]     = "❌ Замовлення скасовано.",
        ["Msg_WaiterAdded"]   = "✅ Офіціанта {0} додано.",
        ["Msg_WaiterRemoved"] = "🗑 Офіціанта видалено.",

        // Демо-дані: категорії
        ["Cat_Soups"]    = "Супи",
        ["Cat_Main"]     = "Головні",
        ["Cat_Grill"]    = "Гриль",
        ["Cat_Salads"]   = "Салати",
        ["Cat_Desserts"] = "Десерти",
        ["Cat_Drinks"]   = "Напої",

        // Демо-дані: страви
        ["Dish_1_Name"]  = "Борщ",
        ["Dish_1_Desc"]  = "Традиційний червоний борщ зі сметаною",
        ["Dish_2_Name"]  = "Солянка",
        ["Dish_2_Desc"]  = "М'ясна солянка з оливками та лимоном",
        ["Dish_3_Name"]  = "Вареники",
        ["Dish_3_Desc"]  = "З картоплею і цибулею, смажені",
        ["Dish_4_Name"]  = "Котлета по-київськи",
        ["Dish_4_Desc"]  = "Куряча котлета з вершковим маслом",
        ["Dish_5_Name"]  = "Стейк рибай",
        ["Dish_5_Desc"]  = "300г, середнє прожарення",
        ["Dish_6_Name"]  = "Лосось на грилі",
        ["Dish_6_Desc"]  = "250г, з овочами та лимонним соусом",
        ["Dish_7_Name"]  = "Цезар",
        ["Dish_7_Desc"]  = "Салат з куркою, пармезаном і сухариками",
        ["Dish_8_Name"]  = "Грецький салат",
        ["Dish_8_Desc"]  = "Свіжі овочі, оливки, фета",
        ["Dish_9_Name"]  = "Тірамісу",
        ["Dish_9_Desc"]  = "Класичний італійський десерт",
        ["Dish_10_Name"] = "Чізкейк",
        ["Dish_10_Desc"] = "Нью-Йоркський чізкейк з ягодами",
        ["Dish_11_Name"] = "Американо",
        ["Dish_11_Desc"] = "Подвійний еспресо з водою",
        ["Dish_12_Name"] = "Апельсиновий сік",
        ["Dish_12_Desc"] = "Свіжовичавлений, 300мл",

        // Демо-дані: офіціанти
        ["Waiter_1_Name"] = "Олена Коваль",
        ["Waiter_2_Name"] = "Дмитро Шевченко",
        ["Waiter_3_Name"] = "Марія Бондар",
    };

    public static readonly Dictionary<string, string> En = new()
    {
        // Header and tabs
        ["Header_Subtitle"]   = "Restaurant management system",
        ["Stat_ActiveOrders"] = "Active orders",
        ["Stat_Dishes"]       = "Dishes on the menu",
        ["Stat_Waiters"]      = "Waiters",
        ["Stat_Revenue"]      = "Revenue today",
        ["Tab_Orders"]        = "📝 Orders",
        ["Tab_Menu"]          = "🍕 Menu",
        ["Tab_Waiters"]       = "👥 Waiters",

        // Order statuses
        ["Status_Pending"]    = "⏳ Pending",
        ["Status_InProgress"] = "👨‍🍳 Preparing",
        ["Status_Served"]     = "✅ Served",
        ["Status_Paid"]       = "💳 Paid",
        ["Status_Cancelled"]  = "❌ Cancelled",

        // Tables, dishes
        ["Table_Free"]        = "🟢 Free",
        ["Table_Occupied"]    = "🔴 Occupied",
        ["Table_Label"]       = "Table #{0} ({1} seats) — {2}",
        ["Dish_Available"]    = "✅ Available",
        ["Dish_Unavailable"]  = "❌ Unavailable",

        // Errors and summaries
        ["Err_Discount"]      = "The discount must be between 0 and 100.",
        ["Err_OrderClosed"]   = "Dishes cannot be added to a closed order.",
        ["Order_Summary"]     = "Order #{0} | Table {1} | {2} | {3:F2} ₴",

        // Orders tab
        ["Orders_Title"]      = "Orders",
        ["Orders_New"]        = "New order",
        ["Orders_Table"]      = "Table",
        ["Orders_Waiter"]     = "Waiter",
        ["Orders_Create"]     = "➕ Create order",
        ["Order_Number"]      = "Order #{0}",
        ["Table_Number"]      = "🪑 Table #{0}",
        ["Orders_Details"]    = "Order details",
        ["Orders_Add"]        = "Add",
        ["Orders_Discount"]   = "Discount (%):",
        ["Orders_ToPay"]      = "Total due:",
        ["Orders_Actions"]    = "Actions",
        ["Orders_StatusChange"] = "Change status",
        ["Orders_ToKitchen"]  = "👨‍🍳  Send to kitchen",
        ["Orders_Served"]     = "✅  Order served",
        ["Orders_Payment"]    = "Payment",
        ["Orders_Pay"]        = "💳  Take payment",
        ["Orders_Cancel"]     = "❌  Cancel order",
        ["Orders_InfoTitle"]  = "ℹ️ Information",
        ["Orders_InfoText"]   = "To take payment, the order must have the status 'Served'. The discount can be set in the central panel.",

        // Menu tab
        ["Filter_All"]        = "All",
        ["Menu_Title"]        = "Menu",
        ["Menu_AddTitle"]     = "Add a dish",
        ["Menu_Emoji"]        = "Emoji",
        ["Menu_Name"]         = "Name *",
        ["Menu_Description"]  = "Description",
        ["Menu_Price"]        = "Price (₴) *",
        ["Menu_Category"]     = "Category *",
        ["Menu_AddButton"]    = "➕ Add to menu",
        ["Menu_RemoveButton"] = "🗑  Delete selected",
        ["Menu_ClearButton"]  = "Clear form",
        ["Menu_Hint"]         = "* — required fields. To edit a dish, click its card — the fields are filled in automatically.",

        // Waiters tab
        ["Waiters_Title"]       = "Waiters",
        ["Waiters_ActiveCount"] = "{0} active",
        ["Waiters_Orders"]      = "Waiter's orders",
        ["Waiters_Completed"]   = "Completed",
        ["Waiters_Revenue"]     = "Revenue",
        ["Waiters_NewTitle"]    = "New waiter",
        ["Waiters_Avatar"]      = "Avatar",
        ["Waiters_Name"]        = "Name *",
        ["Waiters_Phone"]       = "Phone",
        ["Waiters_Add"]         = "➕ Add waiter",
        ["Waiters_Remove"]      = "🗑  Delete selected",
        ["Waiters_Clear"]       = "Clear",

        // Action messages
        ["Msg_OrderCreated"]  = "✅ Order #{0} created for table {1}",
        ["Msg_DishAdded"]     = "➕ Added: {0} × {1}",
        ["Msg_DishRemoved"]   = "🗑 Removed: {0}",
        ["Msg_Status"]        = "Status: {0}",
        ["Msg_Paid"]          = "💳 Paid {0:F2} ₴. Thank you!",
        ["Msg_Cancelled"]     = "❌ Order cancelled.",
        ["Msg_WaiterAdded"]   = "✅ Waiter {0} added.",
        ["Msg_WaiterRemoved"] = "🗑 Waiter removed.",

        // Demo data: categories
        ["Cat_Soups"]    = "Soups",
        ["Cat_Main"]     = "Main courses",
        ["Cat_Grill"]    = "Grill",
        ["Cat_Salads"]   = "Salads",
        ["Cat_Desserts"] = "Desserts",
        ["Cat_Drinks"]   = "Drinks",

        // Demo data: dishes
        ["Dish_1_Name"]  = "Borscht",
        ["Dish_1_Desc"]  = "Traditional red borscht with sour cream",
        ["Dish_2_Name"]  = "Solyanka",
        ["Dish_2_Desc"]  = "Meat solyanka with olives and lemon",
        ["Dish_3_Name"]  = "Varenyky",
        ["Dish_3_Desc"]  = "Potato and onion dumplings, pan-fried",
        ["Dish_4_Name"]  = "Chicken Kyiv",
        ["Dish_4_Desc"]  = "Chicken cutlet with butter",
        ["Dish_5_Name"]  = "Ribeye steak",
        ["Dish_5_Desc"]  = "300 g, medium",
        ["Dish_6_Name"]  = "Grilled salmon",
        ["Dish_6_Desc"]  = "250 g, with vegetables and lemon sauce",
        ["Dish_7_Name"]  = "Caesar salad",
        ["Dish_7_Desc"]  = "Salad with chicken, parmesan and croutons",
        ["Dish_8_Name"]  = "Greek salad",
        ["Dish_8_Desc"]  = "Fresh vegetables, olives, feta",
        ["Dish_9_Name"]  = "Tiramisu",
        ["Dish_9_Desc"]  = "Classic Italian dessert",
        ["Dish_10_Name"] = "Cheesecake",
        ["Dish_10_Desc"] = "New York cheesecake with berries",
        ["Dish_11_Name"] = "Americano",
        ["Dish_11_Desc"] = "Double espresso with water",
        ["Dish_12_Name"] = "Orange juice",
        ["Dish_12_Desc"] = "Freshly squeezed, 300 ml",

        // Demo data: waiters
        ["Waiter_1_Name"] = "Olena Koval",
        ["Waiter_2_Name"] = "Dmytro Shevchenko",
        ["Waiter_3_Name"] = "Maria Bondar",
    };
}
