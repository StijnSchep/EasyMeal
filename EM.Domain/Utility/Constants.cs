using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Domain.Utility
{
    public static class Constants
    {

        // Required
        public const String NameRequired = "Vul een naam in";
        public const String EmailRequired = "Vul een emailadres in";
        public const String PhoneRequired = "Vul een telefoonnummer in";
        public const String PriceRequired = "Vul een prijs in";
        public const String DescriptionRequired = "Vul een beschrijving in";


        // Invalid
        public const String EmailInvalid = "Emailadres is ongeldig";
        public const String PhoneInvalid = "Telefoonnummer is ongeldig";
        public const String WeekPlanInvalidSmall = "Het weekplan bevat niet voor iedere dag een maaltijd";
        public const String WeekPlanInvalidBig = "Het weekplan bevat te veel maaltijden";


        // Misc
        public const String WeekPlanDietaryRepresentation = "Voor elke dieetbeperking moet per dag een maaltijd beschikbaar zijn";


        // Dietary Restrictions
        public static string[] DietaryRestrictions = {
            "Zoutloos",
            "Diabetes",
            "Glutenallergie"
        };

        public static string[] categories =
        {
            "Voorgerecht",
            "Hoofdgerecht",
            "Nagerecht"
        };

    }
}
