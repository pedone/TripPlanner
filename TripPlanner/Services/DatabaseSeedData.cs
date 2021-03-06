﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TripPlanner.Data;
using TripPlanner.Interfaces;
using TripPlanner.Models;
using TripPlanner.Options;
using TimeZone = TripPlanner.Models.TimeZone;

namespace TripPlanner.Services
{
    public class DatabaseSeedData : IDatabaseSeedData
    {
        //The main 'geoname' table has the following fields :
        //---------------------------------------------------
        //geonameid         : integer id of record in geonames database
        //name              : name of geographical point(utf8) varchar(200)
        //asciiname         : name of geographical point in plain ascii characters, varchar(200)
        //alternatenames    : alternatenames, comma separated, ascii names automatically transliterated, convenience attribute from alternatename table, varchar(10000)
        //latitude          : latitude in decimal degrees(wgs84)
        //longitude         : longitude in decimal degrees(wgs84)
        //feature class     : see http://www.geonames.org/export/codes.html, char(1)
        //feature code      : see http://www.geonames.org/export/codes.html, varchar(10)
        //country code      : ISO-3166 2-letter country code, 2 characters
        //cc2               : alternate country codes, comma separated, ISO-3166 2-letter country code, 200 characters
        //admin1 code       : fipscode (subject to change to iso code), see exceptions below, see file admin1Codes.txt for display names of this code; varchar(20)
        //admin2 code       : code for the second administrative division, a county in the US, see file admin2Codes.txt; varchar(80) 
        //admin3 code       : code for third level administrative division, varchar(20)
        //admin4 code       : code for fourth level administrative division, varchar(20)
        //population        : bigint(8 byte int)
        //elevation         : in meters, integer
        //dem               : digital elevation model, srtm3 or gtopo30, average elevation of 3''x3'' (ca 90mx90m) or 30''x30'' (ca 900mx900m) area in meters, integer.srtm processed by cgiar/ciat.
        //timezone          : the iana timezone id(see file timeZone.txt) varchar(40)
        //modification date : date of last modification in yyyy-MM-dd format

        private enum GeonameField
        {
            Id,
            Name,
            AsciiName,
            AlternateNames,
            Latitude,
            Longitude,
            FeatureClass,
            FeatureCode,
            CountryCode,
            AlternateCountryCodes,
            AdminCode,
            AdminCode2,
            AdminCode3,
            AdminCode4,
            Population,
            Elevation,
            DigitalElevationModel,
            TimeZone,
            ModificationDate,
            Count
        }

        private DatabaseUpdateOptions _options;
        private TripPlannerContext _context;
        private ILogger _logger;

        public DatabaseSeedData(IOptions<DatabaseUpdateOptions> options, TripPlannerContext context, ILogger<DatabaseSeedData> logger)
        {
            _options = options.Value;
            _context = context;
            _logger = logger;
        }

        private void ClearDB()
        {
            _context.Countries.RemoveRange(_context.Countries);
            _context.TimeZones.RemoveRange(_context.TimeZones);
            _context.FeatureCodes.RemoveRange(_context.FeatureCodes);
            _context.FeatureCategories.RemoveRange(_context.FeatureCategories);
            _context.GeoData.RemoveRange(_context.GeoData);
        }

        public void InitializeDatabase()
        {
            ClearDB();

            //FeatureCodes, FeatureCategories
            string featureCodeFile = Path.Combine(_options.SeedDataPath, _options.FeatureCodeFile);
            var features = ReadFeatureData(featureCodeFile);

            //CountryCodes
            string countryCodeFile = Path.Combine(_options.SeedDataPath, _options.CountryCodeFile);
            IEnumerable<Country> countries = ReadCountryData(countryCodeFile).ToList();

            //TimeZones
            string timeZoneFile = Path.Combine(_options.SeedDataPath, _options.TimeZoneFile);
            IEnumerable<TimeZone> timezones = ReadTimeZoneData(timeZoneFile, countries).ToList();

            string geoDataFile = Path.Combine(_options.SeedDataPath, _options.GeoDataFile);
            var geoData = InitGeoData<GeoData>(geoDataFile, features.codes, countries, timezones).ToList();

            //insert data
            _context.FeatureCodes.AddRange(features.codes);
            _context.FeatureCategories.AddRange(features.categories);
            _context.TimeZones.AddRange(timezones);
            _context.Countries.AddRange(countries);
            _context.GeoData.AddRange(geoData);

            _context.SaveChanges();
        }

        private IEnumerable<TimeZone> ReadTimeZoneData(string file, IEnumerable<Country> countries)
        {
            using (var reader = new StreamReader(file))
            {
                int index = 0;
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var parts = line.Split(Constants.Separator.Tab);
                    if (parts.Length != 5)
                        throw new InvalidDataException("the time zone data does not have the correct format");

                    yield return new TimeZone
                    {
                        Id = index,
                        Name = parts[1],
                        GMT = double.Parse(parts[2]),
                        Country = countries.First(cur => cur.Code == parts[0])
                    };
                    index++;
                }
            }
        }

        private IEnumerable<Country> ReadCountryData(string file)
        {
            using (var reader = new StreamReader(file))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var parts = line.Split(Constants.Separator.Comma);
                    if (parts.Length != 2)
                        throw new InvalidDataException("the country code data does not have the correct format");

                    yield return new Country
                    {
                        Name = parts[0],
                        Code = parts[1]
                    };
                }
            }
        }

        private (IEnumerable<FeatureCode> codes, IEnumerable<FeatureCategory> categories) ReadFeatureData(string file)
        {
            var categories = new List<FeatureCategory>();
            var codes = new List<FeatureCode>();

            using (var reader = new StreamReader(file))
            {
                FeatureCategory curCategory = null;
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var parts = line.Split(Constants.Separator.Tab);
                    switch (parts.Length)
                    {
                        //Feature Category
                        case 1:
                            parts = parts[0].Split(new char[] { ' ' }, 2);
                            if (parts.Length != 2 || parts[0].Length != 1)
                                throw new InvalidDataException("the feature category data does not have the correct format");

                            curCategory = new FeatureCategory
                            {
                                Code = parts[0],
                                Name = parts[1]
                            };
                            categories.Add(curCategory);
                            break;
                        case 3:
                            if (curCategory == null)
                                throw new InvalidDataException("feature category not set");

                            codes.Add(new FeatureCode
                            {
                                Code = parts[0],
                                Name = parts[1],
                                Description = parts[2],
                                FeatureCategory = curCategory
                            });
                            break;
                        default:
                            throw new InvalidDataException("unexpected feature code format");
                    }
                }
            }

            return (codes: codes, categories: categories);
        }

        private IEnumerable<T> InitGeoData<T>(string file, IEnumerable<FeatureCode> featureCodes, IEnumerable<Country> countries, IEnumerable<TimeZone> timezones)
            where T : GeoData, new()
        {
            using (var reader = new StreamReader(file))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var parts = line.Split("\t");
                    if (parts.Length != (int)GeonameField.Count)
                        throw new InvalidDataException("geo data does not have the correct format");

                    yield return new T
                    {
                        Id = int.Parse(parts[(int)GeonameField.Id]),
                        Name = parts[(int)GeonameField.Name],
                        Lattitude = float.Parse(parts[(int)GeonameField.Latitude]),
                        Longitude = float.Parse(parts[(int)GeonameField.Longitude]),
                        AlternateNames = parts[(int)GeonameField.AlternateNames].Split(","),
                        Population = int.Parse(parts[(int)GeonameField.Population]),
                        TimeZone = timezones.First(cur => cur.Name == parts[(int)GeonameField.TimeZone]),
                        Country = countries.First(cur => cur.Code == parts[(int)GeonameField.CountryCode]),
                        FeatureCode = featureCodes.First(cur => cur.Code == parts[(int)GeonameField.FeatureCode]),
                    };
                }
            }
        }
    }
}