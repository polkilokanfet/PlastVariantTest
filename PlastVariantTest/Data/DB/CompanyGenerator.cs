using System.Collections;

namespace PlastVariantTest.Data.DB;

class CompanyGenerator
{
    private readonly Random _random = new Random();

    // Списки для генерации названий компаний
    private readonly string[] _namePrefixes = { "ООО", "АО", "ЗАО", "ПАО" };
    private readonly string[] _nameWords = {
        "Техно", "Рос", "Сиб", "Урал", "Север", "Юг", "Восток", "Запад", "Пластик",
        "Нефть", "Газ", "Металл", "Строй", "Транс", "Энерго", "Агро", "Телеком",
        "Инвест", "Фин", "Бизнес", "Профи", "Элит", "Гранд", "Мир", "Центр", "Косолапов", "Бердников"
    };
    private readonly string[] _nameSuffixes = {
        "Групп", "Холдинг", "Компания", "Корпорация", "Индастриз", "Сервис",
        "Ресурс", "Пром", "Торг", "Логистика", "Консалтинг", "Девелопмент", "Капитал"
    };

    // Список реальных российских городов
    private readonly string[] _cities = {
        "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань",
        "Нижний Новгород", "Челябинск", "Самара", "Омск", "Ростов-на-Дону",
        "Уфа", "Красноярск", "Воронеж", "Пермь", "Волгоград",
        "Краснодар", "Саратов", "Тюмень", "Тольятти", "Ижевск",
        "Барнаул", "Ульяновск", "Иркутск", "Хабаровск", "Ярославль",
        "Владивосток", "Махачкала", "Томск", "Оренбург", "Кемерово",
        "Новокузнецк", "Рязань", "Астрахань", "Пенза", "Липецк",
        "Киров", "Чебоксары", "Тула", "Калининград", "Брянск",
        "Курск", "Иваново", "Магнитогорск", "Тверь", "Ставрополь",
        "Сочи", "Белгород", "Нижний Тагил", "Владимир", "Архангельск"
    };

    // Генерация случайной компании
    public Company GenerateCompany()
    {
        return new Company
        {
            Name = GenerateCompanyName(),
            City = GenerateCity()
        };
    }

    // Генерация названия компании
    private string GenerateCompanyName()
    {
        return $"{GenerateCompanyNameWithoutPrefix()} ({GetRandomElement(_namePrefixes)})";
        int nameType = _random.Next(0, 3);

        switch (nameType)
        {
            case 0: // Полное название с префиксом
                return $"{GetRandomElement(_namePrefixes)} \"{GenerateCompanyNameWithoutPrefix()}\"";

            case 1: // Без префикса, но с указанием типа
                return $"{GenerateCompanyNameWithoutPrefix()} {GetRandomElement(_nameSuffixes)}";

            default: // Просто название
                return GenerateCompanyNameWithoutPrefix();
        }
    }

    // Генерация названия без префикса
    private string GenerateCompanyNameWithoutPrefix()
    {
        int wordCount = _random.Next(1, 4);
        var words = new List<string>();

        for (int i = 0; i < wordCount; i++)
        {
            words.Add(GetRandomElement(_nameWords));
        }

        // Убираем возможные повторы слов
        for (int i = 0; i < words.Count - 1; i++)
        {
            if (words[i] == words[i + 1])
            {
                words.RemoveAt(i);
                i--;
            }
        }

        return string.Join("", words);
    }

    // Генерация города
    private string GenerateCity()
    {
        return GetRandomElement(_cities);
    }

    // Генерация нескольких компаний
    public List<Company> GenerateCompanies(int count)
    {
        var companies = new List<Company>();

        for (int i = 0; i < count; i++)
        {
            companies.Add(GenerateCompany());
        }

        return companies;
    }

    // Вспомогательный метод для получения случайного элемента массива
    private T GetRandomElement<T>(T[] array)
    {
        return array[_random.Next(array.Length)];
    }
}

