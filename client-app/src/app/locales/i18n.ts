import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        translation: {
          // Basic app translations
          addPost: "Add Post",
          category: "Category",
          categories: "Categories",
          profile: "Profile",
          changeLanguage: "Zmień na PL",
          notLogin: "Login",
          login: "Login",
          loginSuccess: "You have logged in successfully",
          loginFailed: "Login failed",
          logout: "Logout",
          logoutSuccess: "You have logged out successfully",
          logoutFailed: "Logout failed",
          orLogin: "Or login with",
          orRegister: "Or sign in",
          signIn: "Sign in",
          password: "Password",
          passwordConfirm: "Confirm Password",
          firstName: "First Name",
          lastName: "Last Name",
          country: "Country",
          city: "City",
          street: "Street",
          houseNumber: "House number",
          postalCode: "Postal code",
          passwordConfirmFailed: "Password field mismatch",
          signInSuccess: "You have signed in successfully",
          signInFailed: "Sign in has failed",
          settings: "Settings",
          thankForSignIn: "Thank you for sign in!",

          // Post creation translations
          postType: "Post Type",
          basicInformation: "Basic Information",
          title: "Title",
          shortDescription: "Short Description",
          fullDescription: "Full Description",
          selectCategory: "Select Category",
          selectSubcategory: "Select Subcategory",
          location: "Location",
          region: "Region",
          buildingNumber: "Building Number",
          propertyDetails: "Property Details",
          workDetails: "Work Details",
          tags: "Tags",
          addTag: "Add Tag",
          add: "Add",
          images: "Images",
          mainImage: "Main Image",
          additionalImages: "Additional Images",
          selectMainImage: "Select Main Image",
          selectAdditionalImages: "Select Additional Images",
          clickToSelectImage: "Click to select image",
          clickToSelectMultipleImages: "Click to select multiple images",
          selectedImages: "Selected Images",
          cancel: "Cancel",
          createPost: "Create Post",
          creating: "Creating...",

          // Email confirmation translations
          emailConfirmationPostRegister:
            "To complete your registration, please confirm your email address by clicking the link in the email we sent you. If you didn't receive the email, please check your spam folder or click below to resend the email.",
          resendEmailConfirmation: "Resend email",
          confirmationMailSent: "New confirmation mail has been sent",
          confirmationMailSuccess: "Email has confirmed successfully",
          confirmationMailFailed: "Email confirmation failed",
          confirmationMailError: "Error while sending mail",
          sendAgain: "Send again in",

          // General UI translations
          hello: "Hello",
          feed: "Feed",
          popular: "Popular",
          loading: "Loading",
          negotiable: "Negotiable",
          Free: "Free",
          from: "from",
          upTo: "up to",
          failedToLoadPosts: "Failed to load posts",
          postNotFound: "Post not found",
          failedToLoadPost: "Failed to load post",
          noPosts: "No posts available",
          common:"Common",
          // Work experience translations
          ExperienceRequired: "Experience required",
          ExperienceNotRequired: "No experience required",
          experienceRequired: "Experience Required",

          // Property translations
          rooms: "Number of rooms",
          numberOfRooms: "Number of Rooms",
          floor: "Floor",
          area: "Area",
          areaSquareMeters: "Area (m²)",

          // Price type translations
          "Onetime Payment": "One-time Payment",
          "Per Hour": "Per Hour",
          "Per Month": "Per Month",
          "Per Day": "Per Day",
          "Per Item": "Per Item",
          priceType: "Price Type",
          price: "Price",
          salary: "Salary",
          minSalary: "Minimum Salary",
          maxSalary: "Maximum Salary",
          currency: "Currency",

          // Property type translations
          WareHouse: "Warehouse",
          Room: "Room",
          Other: "Other",
          House: "House",
          Apartment: "Apartment",
          Office: "Office",
          Studio: "Studio",
          propertyType: "Property Type",

          // Work type translations
          "Full-Time": "Full-Time",
          "Part-Time": "Part-Time",
          Freelance: "Freelance",
          Internship: "Internship",
          Shift: "Shift Work",
          Seasonal: "Seasonal",
          workload: "Workload",

          // Contract type translations
          "Employment Contract": "Employment Contract",
          "Task Contract": "Task Contract",
          "Specific Work Contract": "Specific Work Contract",
          "Volunteer Contract": "Volunteer Contract",
          B2B: "B2B",

          // Work location translations
          OnSite: "On-Site",
          Remote: "Remote",
          Hybrid: "Hybrid",
          workLocation: "Work Location",

          // Post types
          work: "Work",
          rent: "Rent",
          service: "Service",

          // Navigation translations
          Home: "Home",
          home: "Home",
          search: "Search",
          searchPlaceholder: "Search posts, categories...",
          notifications: "Notifications",
          user: "User",
          menu: "Menu",
          pleaseLogin: "Please log in to access this feature",

          // Footer translations
          footerDescription: "Your trusted platform for finding everything you need - from jobs and housing to services and products.",
          quickLinks: "Quick Links",
          moreInfo: "More Info",
          contact: "Contact",
          about: "About",
          help: "Help",
          privacy: "Privacy Policy",
          terms: "Terms of Service",
          availableLanguages: "Available Languages",
          madeWithLove: "Made with love by Anton Krymouski",

          // Post interactions
          share: "Share",
          favorite: "Add to favorites",
          views: "views",
          anonymous: "Anonymous",
          trending: "Trending",
          posts: "posts",
          posts_one: "post",
          posts_other: "posts",
          description: "Description",
          postImage: "Post image",
          viewAllPhotos: "View all photos",
          imageGallery: "Image Gallery",
          of: "of",
          loadingImages: "Loading images",
          noImagesAvailable: "No images available",
          imageLoadError: "Failed to load image",
          image: "Image",
          fullscreen: "Fullscreen",

          // Enhanced UI
          noPostsDescription: "Check back later for new posts",

          // Promotion types
          promotion: {
            Highlight: "Highlighted",
            Top: "Top Post",
          },

          postOwner: "Post Owner",
          contactOwner: "Contact",
          memberSince: "Member since",
          userNotAvailable: "User information not available",

          goBack: "Go Back",

          // Actions
          addToFavorites: "Add to Favorites",

          // General
          free: "Free",

          // Validation translations
          validation: {
            required: "This field is required",
            invalidEmail: "Please enter a valid email address",
            passwordMinLength: "Password must be at least 6 characters long",
            nameMinLength: "Name must be at least 2 characters long",
            passwordsDoNotMatch: "Passwords do not match",
          },

          // Auth translations
          auth: {
            welcome: "Welcome",
            subtitle: "Login to your account or create a new one",
            login: "Login",
            register: "Register",
            email: "Email",
            password: "Password",
            confirmPassword: "Confirm Password",
            firstName: "First Name",
            lastName: "Last Name",
            loginSuccess: "Successfully logged in!",
            loginError: "Login failed. Please check your credentials.",
            registerSuccess: "Registration successful! Please check your email to verify your account.",
            registerError: "Registration failed. Please try again.",
            googleLoginSuccess: "Successfully logged in with Google!",
            googleLoginError: "Google login failed. Please try again.",
            continueWithGoogle: "Continue with Google",
            or: "OR",
          },

          // Email confirmation extended
          emailConfirmation: {
            title: "Email Confirmation Required",
            message: "To complete your registration, please confirm your email address by clicking the link in the email we sent you. The link will expire in 24 hours for security reasons.",
            didntReceive: "Didn't receive the email?",
            resendEmail: "Resend Verification Email",
            resendIn: "You can resend the email in",
            resendAvailable: "You can resend the email in",
            sending: "Sending...",
            emailsSent: "Verification emails sent: {{count}}",
            backToLogin: "Back to Login",
            checkSpamTip: "Don't forget to check your spam/junk folder if you don't see the email in your inbox.",
            noEmailProvided: "No email address provided",
          },
        },
      },
      pl: {
        translation: {
          // Basic app translations
          addPost: "Dodaj Ogłoszenie",
          category: "Kategoria",
          categories: "Kategorie",
          profile: "Profil użytkownika",
          changeLanguage: "Change to EN",
          notLogin: "Logowanie",
          login: "Zaloguj się",
          loginSuccess: "Zalogowałeś się pomyślnie",
          loginFailed: "Nieudana próba logowania",
          logout: "Wyloguj się",
          logoutSuccess: "Zostałeś pomyślnie wylogowany",
          logoutFailed: "Nieudana próba wylogowania",
          orLogin: "Lub zaloguj się za pomocą",
          orRegister: "Lub zarejestruj się",
          signIn: "Zarejestruj się",
          password: "Hasło",
          passwordConfirm: "Potwierdź hasło",
          firstName: "Imię",
          lastName: "Nazwisko",
          country: "Państwo",
          city: "Miasto",
          street: "Ulica",
          houseNumber: "Numer budynku",
          postalCode: "Kod pocztowy",
          passwordConfirmFailed: "Hasło się nie zgadza",
          signInSuccess: "Rejestracja zakończona pomyślnie",
          signInFailed: "Nieudana próba rejestracji",
          settings: "Ustawienia",
          thankForSignIn: "Dziękujemy za rejestrację!",

          // Post creation translations
          postType: "Typ Ogłoszenia",
          basicInformation: "Podstawowe Informacje",
          title: "Tytuł",
          shortDescription: "Krótki Opis",
          fullDescription: "Pełny Opis",
          selectCategory: "Wybierz Kategorię",
          selectSubcategory: "Wybierz Podkategorię",
          location: "Lokalizacja",
          region: "Region",
          buildingNumber: "Numer Budynku",
          propertyDetails: "Szczegóły Nieruchomości",
          workDetails: "Szczegóły Pracy",
          tags: "Tagi",
          addTag: "Dodaj Tag",
          add: "Dodaj",
          images: "Zdjęcia",
          mainImage: "Zdjęcie Główne",
          additionalImages: "Dodatkowe Zdjęcia",
          selectMainImage: "Wybierz Zdjęcie Główne",
          selectAdditionalImages: "Wybierz Dodatkowe Zdjęcia",
          clickToSelectImage: "Kliknij aby wybrać zdjęcie",
          clickToSelectMultipleImages: "Kliknij aby wybrać wiele zdjęć",
          selectedImages: "Wybrane Zdjęcia",
          cancel: "Anuluj",
          createPost: "Utwórz Ogłoszenie",
          creating: "Tworzenie...",

          // Email confirmation translations
          emailConfirmationPostRegister:
            "Aby zakończyć rejestrację, prosimy o potwierdzenie swojego adresu e-mail, klikając link w wiadomości, którą wysłaliśmy. Jeśli nie otrzymałeś wiadomości, sprawdź folder ze spamem lub kliknij poniżej, aby wysłać wiadomość ponownie.",
          resendEmailConfirmation: "Wyślij ponownie",
          confirmationMailSent: "Nowa wiadomość potwierdzająca została wysłana",
          confirmationMailSuccess: "Adres email został potwierdzony",
          confirmationMailFailed: "Nieudana próba potwierdzenia adresu email",
          confirmationMailError: "Nieudana próba wysłania",
          sendAgain: "Wyślij ponownie za",

          // General UI translations
          hello: "Witaj",
          feed: "Polecane",
          popular: "Popularne",
          loading: "Ładowanie",
          negotiable: "Do uzgodnienia",
          Free: "Darmowe",
          from: "od",
          upTo: "do",
          failedToLoadPosts: "Nie udało się załadować ogłoszeń",
          postNotFound: "Ogłoszenie nie znalezione",
          failedToLoadPost: "Nie udało się załadować ogłoszenia",
          noPosts: "Brak dostępnych ogłoszeń",
          common:"Ogólne",

          // Work experience translations
          ExperienceRequired: "Wymagane doświadczenie",
          ExperienceNotRequired: "Bez doświadczenia",
          experienceRequired: "Wymagane Doświadczenie",

          // Property translations
          rooms: "Liczba pokoi",
          numberOfRooms: "Liczba Pokoi",
          floor: "Piętro",
          area: "Powierzchnia",
          areaSquareMeters: "Powierzchnia (m²)",

          // Price type translations
          "Onetime Payment": "Płatność jednorazowa",
          "Per Hour": "Za godzinę",
          "Per Month": "Za miesiąc",
          "Per Day": "Za dzień",
          "Per Item": "Za sztukę",
          priceType: "Typ Ceny",
          price: "Cena",
          salary: "Wynagrodzenie",
          minSalary: "Minimalne Wynagrodzenie",
          maxSalary: "Maksymalne Wynagrodzenie",
          currency: "Waluta",

          // Property type translations
          WareHouse: "Magazyn",
          Room: "Pokój",
          Other: "Inne",
          House: "Dom",
          Apartment: "Mieszkanie",
          Office: "Biuro",
          Studio: "Kawalerka",
          propertyType: "Typ Nieruchomości",

          // Work type translations
          "Full-Time": "Pełny etat",
          "Part-Time": "Niepełny etat",
          Freelance: "Freelance",
          Internship: "Praktyki",
          Shift: "Zmianowa",
          Seasonal: "Sezonowa",
          workload: "Wymiar Pracy",

          // Contract type translations
          "Employment Contract": "Umowa o pracę",
          "Task Contract": "Umowa zlecenie",
          "Specific Work Contract": "Umowa o dzieło",
          "Volunteer Contract": "Umowa wolontariacka",
          B2B: "B2B",

          // Work location translations
          OnSite: "W biurze",
          Remote: "Zdalnie",
          Hybrid: "Hybrydowo",
          workLocation: "Lokalizacja Pracy",

          // Post types
          work: "Praca",
          rent: "Wynajem",
          service: "Usługa",

          // Navigation translations
          Home: "Strona Główna",
          home: "Strona główna",
          search: "Szukaj",
          searchPlaceholder: "Szukaj ogłoszeń, kategorii...",
          notifications: "Powiadomienia",
          user: "Użytkownik",
          menu: "Menu",
          pleaseLogin: "Zaloguj się, aby uzyskać dostęp do tej funkcji",

          // Footer translations
          footerDescription: "Twoja zaufana platforma do znajdowania wszystkiego czego potrzebujesz - od pracy i mieszkań po usługi i produkty.",
          quickLinks: "Szybkie Linki",
          moreInfo: "Dodatkowe Informacje", 
          contact: "Kontakt",
          about: "O Nas",
          privacy: "Polityka Prywatności",
          terms: "Regulamin",
          help: "Centrum Pomocy",
          availableLanguages: "Dostępne Języki",
          poweredBy: "Powered by",
          allRightsReserved: "Wszystkie prawa zastrzeżone",
          madeWithLove: "Stworzono z miłością przez Anton Krymouski",

          // Post interactions
          share: "Udostępnij",
          favorite: "Dodaj do ulubionych",
          views: "wyświetleń",
          anonymous: "Anonimowy",
          trending: "Popularne",
          posts: "ogłoszeń",
          posts_one: "ogłoszenie",
          posts_other: "ogłoszenia",
          description: "Opis",
          postImage: "Zdjęcie ogłoszenia",
          viewAllPhotos: "Zobacz wszystkie zdjęcia",
          imageGallery: "Galeria Zdjęć",
          of: "z",
          loadingImages: "Ładowanie zdjęć",
          noImagesAvailable: "Brak dostępnych zdjęć",
          imageLoadError: "Nie udało się załadować zdjęcia",
          image: "Zdjęcie",
          fullscreen: "Pełny ekran",

          // Enhanced UI
          noPostsDescription: "Sprawdź ponownie później czy pojawiły się nowe ogłoszenia",

          // Promotion types
          promotion: {
            Highlight: "Wyróżnione",
            Top: "Top Ogłoszenie",
          },

          postOwner: "Ogłoszeniodawca",
          contactOwner: "Skontaktuj się",
          memberSince: "Członek od",
          userNotAvailable: "Informacje o użytkowniku niedostępne",

          // Navigation
          goBack: "Wróć",

          // Actions
          addToFavorites: "Dodaj do Ulubionych",

          // General
          free: "Darmowe",

          // Validation translations
          validation: {
            required: "To pole jest wymagane",
            invalidEmail: "Wprowadź prawidłowy adres email",
            passwordMinLength: "Hasło musi mieć co najmniej 6 znaków",
            nameMinLength: "Imię musi mieć co najmniej 2 znaki",
            passwordsDoNotMatch: "Hasła się nie zgadzają",
          },

          // Auth translations
          auth: {
            welcome: "Witaj",
            subtitle: "Zaloguj się na swoje konto lub utwórz nowe",
            login: "Zaloguj się",
            register: "Zarejestruj się",
            email: "Email",
            password: "Hasło",
            confirmPassword: "Potwierdź Hasło",
            firstName: "Imię",
            lastName: "Nazwisko",
            loginSuccess: "Pomyślnie zalogowano!",
            loginError: "Logowanie nie powiodło się. Sprawdź swoje dane.",
            registerSuccess: "Rejestracja zakończona sukcesem! Sprawdź email aby potwierdzić konto.",
            registerError: "Rejestracja nie powiodła się. Spróbuj ponownie.",
            googleLoginSuccess: "Pomyślnie zalogowano przez Google!",
            googleLoginError: "Logowanie przez Google nie powiodło się. Spróbuj ponownie.",
            continueWithGoogle: "Kontynuuj z Google",
            or: "LUB",
          },

          // Email confirmation extended
          emailConfirmation: {
            title: "Wymagane Potwierdzenie Email",
            message: "Aby zakończyć rejestrację, potwierdź swój adres email klikając w link w wiadomości, którą wysłaliśmy. Link wygaśnie za 24 godziny ze względów bezpieczeństwa.",
            didntReceive: "Nie otrzymałeś emaila?",
            resendEmail: "Wyślij Ponownie Email Weryfikacyjny",
            resendIn: "Możesz wysłać ponownie email za",
            resendAvailable: "Możesz wysłać ponownie email za",
            sending: "Wysyłanie...",
            emailsSent: "Wysłano emaili weryfikacyjnych: {{count}}",
            backToLogin: "Powrót do Logowania",
            checkSpamTip: "Nie zapomnij sprawdzić folderu spam/junk jeśli nie widzisz emaila w skrzynce odbiorczej.",
            noEmailProvided: "Nie podano adresu email",
          },
        },
      },
    },
    lng: localStorage.getItem("brt_lng") || "en",
    fallbackLng: "en",
    interpolation: { escapeValue: false },
  });

export default i18n;