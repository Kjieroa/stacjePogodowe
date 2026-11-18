# Stacja Pogodowa (WPF / C#)

Aplikacja okienkowa stworzona w języku **C#** z wykorzystaniem technologii **WPF (.NET)**, służąca do zarządzania i analizy pomiarów meteorologicznych pochodzących z różnych stacji pogodowych.

## Funkcjonalności

* **Odczyt danych z pliku tekstowego:** Wczytywanie pomiarów z pliku `pomiary.txt` wykorzystującego niestandardowy separator ` ; ` (spacja-średnik-spacja).
* **Prezentacja w tabeli:** Czytelne wyświetlanie danych (stacja, data i czas, temperatura, ciśnienie) w komponencie `DataGrid`.
* **Filtrowanie po stacji:** Dynamicznie uzupełniana lista rozwijana (`ComboBox`), umożliwiająca szybkie filtrowanie pomiarów dla wybranej stacji lub wyświetlenie wszystkich rekordów.
* **Wyróżnianie wartości ekstremalnych:** Automatyczne wykrywanie i podświetlanie w tabeli najwyższej temperatury (**MAX** – kolor czerwony/koralowy) oraz najniższej temperatury (**MIN** – kolor niebieski).
* **Dodawanie nowych pomiarów:** Formularz z walidacją danych pozwalający na dopisanie nowego pomiaru z aktualną datą i godziną.
* **Zapis do pliku:** Możliwość zapisania aktualnego stanu listy pomiarów z powrotem do pliku `pomiary.txt` z zachowaniem wymaganego formatowania.

## Format pliku danych (`pomiary.txt`)

Dane w pliku tekstowym muszą być zapisane w formacie:
`Stacja ; Data i czas ; Temperatura ; Ciśnienie`

Przykładowa zawartość pliku:
```text
Warszawa ; 2026-11-18 08:00 ; 12,5 ; 1015
Kraków ; 2026-11-18 08:00 ; 9,2 ; 1018
Wrocław ; 2026-11-18 08:00 ; 14,8 ; 1012
Warszawa ; 2026-11-18 12:00 ; 16,1 ; 1014
Kraków ; 2026-11-18 12:00 ; 11,0 ; 1017
Gdańsk ; 2026-11-18 12:00 ; 7,4 ; 1020
