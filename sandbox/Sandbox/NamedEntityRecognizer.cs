using System;
using System.Collections.Generic;

namespace MyNlpLibrary
{
    public enum EntityType
    {
        Person,
        Organization,
        Location,
        Date,
        Other,
        Time,
        Money,
        Percent,
        Facility,
        Event,
        Vehicle,
        Quantity,
        Sport,
        School,
        Book,
        Movie,
        Song,
        VideoGame,
        Object,
        Recipe,
        Holiday,
        Instrument,
        Food,
        Drink,
    }

    public class NamedEntityRecognizer
    {
        private readonly Dictionary<string, EntityType> _entities = new Dictionary<string, EntityType>(StringComparer.OrdinalIgnoreCase)
{
    { "PERSON", EntityType.Person },
    { "LOCATION", EntityType.Location },
    { "ORGANIZATION", EntityType.Organization },
    { "DATE", EntityType.Date },
    { "TIME", EntityType.Time },
    { "MONEY", EntityType.Object },
    { "PERCENT", EntityType.Percent },
    { "EVENT", EntityType.Event },
    { "VEHICLE", EntityType.Vehicle },
    { "QUANTITY", EntityType.Quantity },
    { "SPORT", EntityType.Sport },
    { "SCHOOL", EntityType.School },
    { "BOOK", EntityType.Book },
    { "MOVIE", EntityType.Movie },
    { "SONG", EntityType.Song },
    { "VIDEO_GAME", EntityType.VideoGame },
    { "RECIPE", EntityType.Recipe },
    { "HOLIDAY", EntityType.Holiday },
    { "INSTRUMENT", EntityType.Instrument },
    { "FOOD", EntityType.Food },
    { "DRINK", EntityType.Drink },
    { "HOSPITAL", EntityType.Organization }
};
        public List<EntityType> Recognize(List<string> tokens)
        {
            var entities = new List<EntityType>();

            foreach (var token in tokens)
            {
                if (_entities.ContainsKey(token))
                {
                    entities.Add(_entities[token]);
                }
                else
                {
                    entities.Add(EntityType.Other);
                }
            }

            return entities;
        }
    }
}