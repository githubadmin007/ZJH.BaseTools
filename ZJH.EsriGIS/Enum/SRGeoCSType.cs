using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJH.EsriGIS.Enum
{
    //
    // 摘要:
    //     The available geographic coordinate systems.
    public enum SRGeoCSType
    {
        //
        // 摘要:
        //     Airy 1830.
        esriSRGeoCS_Airy1830 = 4001,
        //
        // 摘要:
        //     Airy modified.
        esriSRGeoCS_ModifiedAiry = 4002,
        //
        // 摘要:
        //     Australian National.
        esriSRGeoCS_Australian = 4003,
        //
        // 摘要:
        //     Bessel 1841.
        esriSRGeoCS_Bessel1841 = 4004,
        //
        // 摘要:
        //     Bessel modified.
        esriSRGeoCS_ModifiedBessel = 4005,
        //
        // 摘要:
        //     Bessel Namibia.
        esriSRGeoCS_BesselNamibia = 4006,
        //
        // 摘要:
        //     Clarke 1858.
        esriSRGeoCS_Clarke1858 = 4007,
        //
        // 摘要:
        //     Clarke 1866.
        esriSRGeoCS_Clarke1866 = 4008,
        //
        // 摘要:
        //     Clarke 1866 Michigan.
        esriSRGeoCS_Clarke1866Michigan = 4009,
        //
        // 摘要:
        //     Clarke 1880 (Benoit).
        esriSRGeoCS_Clarke1880Benoit = 4010,
        //
        // 摘要:
        //     Clarke 1880 (IGN).
        esriSRGeoCS_Clarke1880IGN = 4011,
        //
        // 摘要:
        //     Clarke 1880 (RGS).
        esriSRGeoCS_Clarke1880RGS = 4012,
        //
        // 摘要:
        //     Clarke 1880 (Arc).
        esriSRGeoCS_Clarke1880Arc = 4013,
        //
        // 摘要:
        //     Clarke 1880 (SGA).
        esriSRGeoCS_Clarke1880SGA = 4014,
        //
        // 摘要:
        //     Everest (definition 1967).
        esriSRGeoCS_Everest1967 = 4016,
        //
        // 摘要:
        //     Everest modified.
        esriSRGeoCS_ModifiedEverest = 4018,
        //
        // 摘要:
        //     GRS 1980.
        esriSRGeoCS_GRS1980 = 4019,
        //
        // 摘要:
        //     Helmert 1906.
        esriSRGeoCS_Helmert1906 = 4020,
        //
        // 摘要:
        //     Indonesian National.
        esriSRGeoCS_Indonesian = 4021,
        //
        // 摘要:
        //     International 1927.
        esriSRGeoCS_International1924 = 4022,
        //
        // 摘要:
        //     Krasovsky 1940.
        esriSRGeoCS_Krasovsky1940 = 4024,
        //
        // 摘要:
        //     Transit precise ephemeris.
        esriSRGeoCS_NWL9D = 4025,
        //
        // 摘要:
        //     Plessis 1817.
        esriSRGeoCS_Plessis1817 = 4027,
        //
        // 摘要:
        //     Struve 1860.
        esriSRGeoCS_Struve1860 = 4028,
        //
        // 摘要:
        //     War Office.
        esriSRGeoCS_WarOffice = 4029,
        //
        // 摘要:
        //     GEM gravity potential model.
        esriSRGeoCS_GEM10C = 4031,
        //
        // 摘要:
        //     OSU 1986 geoidal model.
        esriSRGeoCS_OSU1986F = 4032,
        //
        // 摘要:
        //     OSU 1991 geoidal model.
        esriSRGeoCS_OSU1991A = 4033,
        //
        // 摘要:
        //     Clarke 1880.
        esriSRGeoCS_Clarke1880 = 4034,
        //
        // 摘要:
        //     Authalic sphere.
        esriSRGeoCS_Authalicsphere = 4035,
        //
        // 摘要:
        //     GRS 1967.
        esriSRGeoCS_GRS1967 = 4036,
        //
        // 摘要:
        //     Everest 1830.
        esriSRGeoCS_Everest1830 = 4042,
        //
        // 摘要:
        //     Everest (definition 1962).
        esriSRGeoCS_Everest1962 = 4044,
        //
        // 摘要:
        //     Everest (definition 1975).
        esriSRGeoCS_Everest1975 = 4045,
        //
        // 摘要:
        //     Greek.
        esriSRGeoCS_Greek = 4120,
        //
        // 摘要:
        //     Greek Geodetic Ref. System 1987.
        esriSRGeoCS_GGRS1987 = 4121,
        //
        // 摘要:
        //     Average Terrestrial System 1977.
        esriSRGeoCS_ATS1977 = 4122,
        //
        // 摘要:
        //     Kartastokoordinaattijarjestelma.
        esriSRGeoCS_KKJ = 4123,
        //
        // 摘要:
        //     RT 1990.
        esriSRGeoCS_RT90 = 4124,
        //
        // 摘要:
        //     Samboja.
        esriSRGeoCS_Samboja = 4125,
        //
        // 摘要:
        //     Tete.
        esriSRGeoCS_Tete = 4127,
        //
        // 摘要:
        //     Madzansua.
        esriSRGeoCS_Madzansua = 4128,
        //
        // 摘要:
        //     Observatario.
        esriSRGeoCS_Observatario = 4129,
        //
        // 摘要:
        //     Moznet.
        esriSRGeoCS_Moznet = 4130,
        //
        // 摘要:
        //     Indian 1960.
        esriSRGeoCS_Indian1960 = 4131,
        //
        // 摘要:
        //     Final Datum 1958.
        esriSRGeoCS_FD1958 = 4132,
        //
        // 摘要:
        //     Estonia 1992.
        esriSRGeoCS_Estonia1992 = 4133,
        //
        // 摘要:
        //     PDO Survey Datum 1993.
        esriSRGeoCS_PDO1993 = 4134,
        //
        // 摘要:
        //     Old Hawaiian.
        esriSRGeoCS_OldHawaiian = 4135,
        //
        // 摘要:
        //     St. Lawrence Island.
        esriSRGeoCS_StLawrenceIsland = 4136,
        //
        // 摘要:
        //     St. Paul Island.
        esriSRGeoCS_StPaulIsland = 4137,
        //
        // 摘要:
        //     St. George Island.
        esriSRGeoCS_StGeorgeIsland = 4138,
        //
        // 摘要:
        //     Puerto Rico.
        esriSRGeoCS_PuertoRico = 4139,
        //
        // 摘要:
        //     Israel.
        esriSRGeoCS_Israel = 4141,
        //
        // 摘要:
        //     Locodjo 1965.
        esriSRGeoCS_Locodjo1965 = 4142,
        //
        // 摘要:
        //     Abidjan 1987.
        esriSRGeoCS_Abidjan1987 = 4143,
        //
        // 摘要:
        //     Kalianpur 1937.
        esriSRGeoCS_Kalianpur1937 = 4144,
        //
        // 摘要:
        //     Kalianpur 1962.
        esriSRGeoCS_Kalianpur1962 = 4145,
        //
        // 摘要:
        //     Kalianpur 1975.
        esriSRGeoCS_Kalianpur1975 = 4146,
        //
        // 摘要:
        //     Hanoi 1972.
        esriSRGeoCS_Hanoi1972 = 4147,
        //
        // 摘要:
        //     Hartebeesthoek 1994.
        esriSRGeoCS_Hartebeesthoek1994 = 4148,
        //
        // 摘要:
        //     CH 1903.
        esriSRGeoCS_CH1903 = 4149,
        //
        // 摘要:
        //     CH 1903+.
        esriSRGeoCS_CH1903Plus = 4150,
        //
        // 摘要:
        //     Chua.
        esriSRGeoCS_CHTRF1995 = 4151,
        //
        // 摘要:
        //     North American Datum 1983 (HARN).
        esriSRGeoCS_NAD1983HARN = 4152,
        //
        // 摘要:
        //     Rassadiran.
        esriSRGeoCS_Rassadiran = 4153,
        //
        // 摘要:
        //     European Datum 1950 (ED77).
        esriSRGeoCS_EuropeanDatum1950ED77 = 4154,
        //
        // 摘要:
        //     Dabola.
        esriSRGeoCS_Dabola = 4155,
        //
        // 摘要:
        //     S-JTSK.
        esriSRGeoCS_S_JTSK = 4156,
        //
        // 摘要:
        //     Bissau.
        esriSRGeoCS_Bissau = 4165,
        //
        // 摘要:
        //     American Samoa 1962.
        esriSRGeoCS_Samoa1962 = 4169,
        //
        // 摘要:
        //     Garoua.
        esriSRGeoCS_Garoua = 4197,
        //
        // 摘要:
        //     Pulkovo 1995.
        esriSRGeoCS_Pulkovo1995 = 4200,
        //
        // 摘要:
        //     Adindan.
        esriSRGeoCS_Adindan = 4201,
        //
        // 摘要:
        //     Australian Geodetic Datum 1966.
        esriSRGeoCS_AGD1966 = 4202,
        //
        // 摘要:
        //     Australian Geodetic Datum 1984.
        esriSRGeoCS_AGD1984 = 4203,
        //
        // 摘要:
        //     Ain el Abd 1970.
        esriSRGeoCS_AinElAbd1970 = 4204,
        //
        // 摘要:
        //     Afgooye.
        esriSRGeoCS_Afgooye = 4205,
        //
        // 摘要:
        //     Agadez.
        esriSRGeoCS_Agadez = 4206,
        //
        // 摘要:
        //     Lisbon.
        esriSRGeoCS_Lisbon = 4207,
        //
        // 摘要:
        //     Aratu.
        esriSRGeoCS_Aratu = 4208,
        //
        // 摘要:
        //     Arc 1950.
        esriSRGeoCS_Arc1950 = 4209,
        //
        // 摘要:
        //     Arc 1960.
        esriSRGeoCS_Arc1960 = 4210,
        //
        // 摘要:
        //     Batavia.
        esriSRGeoCS_Batavia = 4211,
        //
        // 摘要:
        //     Barbados 1938.
        esriSRGeoCS_Barbados1938 = 4212,
        //
        // 摘要:
        //     Beduaram.
        esriSRGeoCS_Beduaram = 4213,
        //
        // 摘要:
        //     Beijing 1954.
        esriSRGeoCS_Beijing1954 = 4214,
        //
        // 摘要:
        //     Reseau National Belge 1950.
        esriSRGeoCS_Belge1950 = 4215,
        //
        // 摘要:
        //     Bermuda 1957.
        esriSRGeoCS_Bermuda1957 = 4216,
        //
        // 摘要:
        //     Bogota.
        esriSRGeoCS_Bogota = 4218,
        //
        // 摘要:
        //     Bukit Rimpah.
        esriSRGeoCS_BukitRimpah = 4219,
        //
        // 摘要:
        //     Camacupa.
        esriSRGeoCS_Camacupa = 4220,
        //
        // 摘要:
        //     Campo Inchauspe.
        esriSRGeoCS_CampoInchauspe = 4221,
        //
        // 摘要:
        //     Cape.
        esriSRGeoCS_Cape = 4222,
        //
        // 摘要:
        //     Carthage.
        esriSRGeoCS_Carthage = 4223,
        //
        // 摘要:
        //     Carthage (degrees).
        esriSRGeoCS_CarthageDegrees = 4223,
        //
        // 摘要:
        //     Chua.
        esriSRGeoCS_CHUA = 4224,
        //
        // 摘要:
        //     Corrego Alegre.
        esriSRGeoCS_CorregoAlegre = 4225,
        //
        // 摘要:
        //     Cote d'Ivoire.
        esriSRGeoCS_CoteDIvoire = 4226,
        //
        // 摘要:
        //     Deir ez Zor.
        esriSRGeoCS_DeirezZor = 4227,
        //
        // 摘要:
        //     Douala.
        esriSRGeoCS_Douala = 4228,
        //
        // 摘要:
        //     Egypt 1907.
        esriSRGeoCS_Egypt1907 = 4229,
        //
        // 摘要:
        //     European Datum 1950.
        esriSRGeoCS_EuropeanDatum1950 = 4230,
        //
        // 摘要:
        //     European Datum 1987.
        esriSRGeoCS_EuropeanDatum1987 = 4231,
        //
        // 摘要:
        //     Fahud.
        esriSRGeoCS_Fahud = 4232,
        //
        // 摘要:
        //     Gandajika 1970.
        esriSRGeoCS_Gandajika1970 = 4233,
        //
        // 摘要:
        //     Guyane Francaise.
        esriSRGeoCS_GuyaneFrancaise = 4235,
        //
        // 摘要:
        //     Hu Tzu Shan.
        esriSRGeoCS_HuTzuShan = 4236,
        //
        // 摘要:
        //     Hungarian Datum 1972.
        esriSRGeoCS_Hungarian1972 = 4237,
        //
        // 摘要:
        //     Indonesian Datum 1974.
        esriSRGeoCS_Indonesian1974 = 4238,
        //
        // 摘要:
        //     Indian 1954.
        esriSRGeoCS_Indian1954 = 4239,
        //
        // 摘要:
        //     Indian 1975.
        esriSRGeoCS_Indian1975 = 4240,
        //
        // 摘要:
        //     Jamaica 1875.
        esriSRGeoCS_Jamaica1875 = 4241,
        //
        // 摘要:
        //     Jamaica 1969.
        esriSRGeoCS_Jamaica1969 = 4242,
        //
        // 摘要:
        //     Kalianpur 1880.
        esriSRGeoCS_Kalianpur1880 = 4243,
        //
        // 摘要:
        //     Kandawala.
        esriSRGeoCS_Kandawala = 4244,
        //
        // 摘要:
        //     Kertau.
        esriSRGeoCS_Kertau = 4245,
        //
        // 摘要:
        //     Kuwait Oil Company.
        esriSRGeoCS_KOC = 4246,
        //
        // 摘要:
        //     La Canoa.
        esriSRGeoCS_LaCanoa = 4247,
        //
        // 摘要:
        //     Provisional South American Datum 1956.
        esriSRGeoCS_PSAD1956 = 4248,
        //
        // 摘要:
        //     Lake.
        esriSRGeoCS_Lake = 4249,
        //
        // 摘要:
        //     Leigon.
        esriSRGeoCS_Leigon = 4250,
        //
        // 摘要:
        //     Liberia 1964.
        esriSRGeoCS_Liberia1964 = 4251,
        //
        // 摘要:
        //     Lome.
        esriSRGeoCS_Lome = 4252,
        //
        // 摘要:
        //     Luzon 1911.
        esriSRGeoCS_Luzon1911 = 4253,
        //
        // 摘要:
        //     Hito XVIII 1963.
        esriSRGeoCS_HitoXVIII1963 = 4254,
        //
        // 摘要:
        //     Herat North.
        esriSRGeoCS_HeratNorth = 4255,
        //
        // 摘要:
        //     Mahe 1971.
        esriSRGeoCS_Mahe1971 = 4256,
        //
        // 摘要:
        //     Makassar.
        esriSRGeoCS_Makassar = 4257,
        //
        // 摘要:
        //     Malongo 1987.
        esriSRGeoCS_Malongo1987 = 4259,
        //
        // 摘要:
        //     Manoca.
        esriSRGeoCS_Manoca = 4260,
        //
        // 摘要:
        //     Merchich.
        esriSRGeoCS_Merchich = 4261,
        //
        // 摘要:
        //     Massawa.
        esriSRGeoCS_Massawa = 4262,
        //
        // 摘要:
        //     Minna.
        esriSRGeoCS_Minna = 4263,
        //
        // 摘要:
        //     Mhast.
        esriSRGeoCS_Mhast = 4264,
        //
        // 摘要:
        //     Monte Mario.
        esriSRGeoCS_MonteMario = 4265,
        //
        // 摘要:
        //     M'poraloko.
        esriSRGeoCS_MPoraloko = 4266,
        //
        // 摘要:
        //     North American Datum 1927.
        esriSRGeoCS_NAD1927 = 4267,
        //
        // 摘要:
        //     NAD Michigan.
        esriSRGeoCS_NADMichigan = 4268,
        //
        // 摘要:
        //     North American Datum 1983.
        esriSRGeoCS_NAD1983 = 4269,
        //
        // 摘要:
        //     Nahrwan 1967.
        esriSRGeoCS_Nahrwan1967 = 4270,
        //
        // 摘要:
        //     Naparima 1972.
        esriSRGeoCS_Naparima1972 = 4271,
        //
        // 摘要:
        //     New Zealand Geodetic Datum 1949.
        esriSRGeoCS_NZGD1949 = 4272,
        //
        // 摘要:
        //     NGO 1948.
        esriSRGeoCS_NGO1948 = 4273,
        //
        // 摘要:
        //     Datum 73.
        esriSRGeoCS_Datum73 = 4274,
        //
        // 摘要:
        //     Nouvelle Triangulation Francaise.
        esriSRGeoCS_NTF = 4275,
        //
        // 摘要:
        //     NSWC 9Z-2.
        esriSRGeoCS_NSWC9Z_2 = 4276,
        //
        // 摘要:
        //     OSGB 1936.
        esriSRGeoCS_OSGB1936 = 4277,
        //
        // 摘要:
        //     OSGB 1970 (SN).
        esriSRGeoCS_OSGB1970SN = 4278,
        //
        // 摘要:
        //     OS (SN) 1980.
        esriSRGeoCS_OSSN1980 = 4279,
        //
        // 摘要:
        //     Padang 1884.
        esriSRGeoCS_Padang1884 = 4280,
        //
        // 摘要:
        //     Palestine 1923.
        esriSRGeoCS_Palestine1923 = 4281,
        //
        // 摘要:
        //     Pointe Noire.
        esriSRGeoCS_PointeNoire = 4282,
        //
        // 摘要:
        //     Geocentric Datum of Australia 1994.
        esriSRGeoCS_GDA1994 = 4283,
        //
        // 摘要:
        //     Pulkovo 1942.
        esriSRGeoCS_Pulkovo1942 = 4284,
        //
        // 摘要:
        //     Qatar.
        esriSRGeoCS_Qatar = 4285,
        //
        // 摘要:
        //     Qatar 1948.
        esriSRGeoCS_Qatar1948 = 4286,
        //
        // 摘要:
        //     Qornoq.
        esriSRGeoCS_Qornoq = 4287,
        //
        // 摘要:
        //     Loma Quintana.
        esriSRGeoCS_LomaQuintana = 4288,
        //
        // 摘要:
        //     Amersfoort.
        esriSRGeoCS_Amersfoort = 4289,
        //
        // 摘要:
        //     Sapper Hill 1943.
        esriSRGeoCS_SapperHill1943 = 4292,
        //
        // 摘要:
        //     Schwarzeck.
        esriSRGeoCS_Schwarzeck = 4293,
        //
        // 摘要:
        //     Segora.
        esriSRGeoCS_Segora = 4294,
        //
        // 摘要:
        //     Serindung.
        esriSRGeoCS_Serindung = 4295,
        //
        // 摘要:
        //     Sudan.
        esriSRGeoCS_Sudan = 4296,
        //
        // 摘要:
        //     Tananarive 1925.
        esriSRGeoCS_Tananarive1925 = 4297,
        //
        // 摘要:
        //     Timbalai 1948.
        esriSRGeoCS_Timbalai1948 = 4298,
        //
        // 摘要:
        //     TM65.
        esriSRGeoCS_TM65 = 4299,
        //
        // 摘要:
        //     TM75.
        esriSRGeoCS_TM75 = 4300,
        //
        // 摘要:
        //     Tokyo.
        esriSRGeoCS_Tokyo = 4301,
        //
        // 摘要:
        //     Trinidad 1903.
        esriSRGeoCS_Trinidad1903 = 4302,
        //
        // 摘要:
        //     Trucial Coast 1948.
        esriSRGeoCS_TrucialCoast1948 = 4303,
        //
        // 摘要:
        //     Voirol 1875.
        esriSRGeoCS_Voirol1875 = 4304,
        //
        // 摘要:
        //     Voirol 1875 (Degree).
        esriSRGeoCS_Voirol1875Degree = 4304,
        //
        // 摘要:
        //     Voirol Unifie 1960.
        esriSRGeoCS_VoirolUnifie1960 = 4305,
        //
        // 摘要:
        //     Bern 1938.
        esriSRGeoCS_Bern1938 = 4306,
        //
        // 摘要:
        //     Nord Sahara 1959.
        esriSRGeoCS_NordSahara1959 = 4307,
        //
        // 摘要:
        //     RT38.
        esriSRGeoCS_RT38 = 4308,
        //
        // 摘要:
        //     Yacare.
        esriSRGeoCS_Yacare = 4309,
        //
        // 摘要:
        //     Yoff.
        esriSRGeoCS_Yoff = 4310,
        //
        // 摘要:
        //     Zanderij.
        esriSRGeoCS_Zanderij = 4311,
        //
        // 摘要:
        //     Militar-Geographische Institut.
        esriSRGeoCS_MGI = 4312,
        //
        // 摘要:
        //     Reseau National Belge 1972.
        esriSRGeoCS_Belge1972 = 4313,
        //
        // 摘要:
        //     Deutsche Hauptdreiecksnetz.
        esriSRGeoCS_DHDN = 4314,
        //
        // 摘要:
        //     Conakry 1905.
        esriSRGeoCS_Conakry1905 = 4315,
        //
        // 摘要:
        //     Dealul Piscului 1933 (Romania).
        esriSRGeoCS_DealulPiscului1933 = 4316,
        //
        // 摘要:
        //     Dealul Piscului 1970 (Romania).
        esriSRGeoCS_DealulPiscului1970 = 4317,
        //
        // 摘要:
        //     National Geodetic Network (Kuwait).
        esriSRGeoCS_NGN = 4318,
        //
        // 摘要:
        //     Kuwait Utility.
        esriSRGeoCS_KUDAMS = 4319,
        //
        // 摘要:
        //     WGS 1972.
        esriSRGeoCS_WGS1972 = 4322,
        //
        // 摘要:
        //     WGS 1972 Transit Broadcast Ephemer.
        esriSRGeoCS_WGS1972BE = 4324,
        //
        // 摘要:
        //     WGS 1984.
        esriSRGeoCS_WGS1984 = 4326,
        //
        // 摘要:
        //     Anguilla 1957.
        esriSRGeoCS_Anguilla1957 = 4600,
        //
        // 摘要:
        //     Antigua Astro 1943.
        esriSRGeoCS_Antigua1943 = 4601,
        //
        // 摘要:
        //     Dominica 1945.
        esriSRGeoCS_Dominica1945 = 4602,
        //
        // 摘要:
        //     Grenada 1953.
        esriSRGeoCS_Grenada1953 = 4603,
        //
        // 摘要:
        //     Montserrat Astro 1958.
        esriSRGeoCS_Montserrat1958 = 4604,
        //
        // 摘要:
        //     St. Kitts 1955.
        esriSRGeoCS_StKitts1955 = 4605,
        //
        // 摘要:
        //     St. Lucia 1955.
        esriSRGeoCS_StLucia1955 = 4606,
        //
        // 摘要:
        //     St. Vincent 1945.
        esriSRGeoCS_StVincent1945 = 4607,
        //
        // 摘要:
        //     NAD 1927 Definition 1976.
        esriSRGeoCS_NAD1927Def1976 = 4608,
        //
        // 摘要:
        //     NAD 1927 CGQ77.
        esriSRGeoCS_NAD1927CGQ77 = 4609,
        //
        // 摘要:
        //     Gunung Segara.
        esriSRGeoCS_GunungSegara = 4613,
        //
        // 摘要:
        //     Porto Santo 1936.
        esriSRGeoCS_PortoSanto1936 = 4615,
        //
        // 摘要:
        //     Selvagem Grande 1938.
        esriSRGeoCS_SelvagemGrande1938 = 4616,
        //
        // 摘要:
        //     NAD 1983 (Canadian SRS 1998).
        esriSRGeoCS_NAD1983CSRS98 = 4617,
        //
        // 摘要:
        //     South American Datum 1969.
        esriSRGeoCS_SAD1969 = 4618,
        //
        // 摘要:
        //     Point 58.
        esriSRGeoCS_Point58 = 4620,
        //
        // 摘要:
        //     Reunion.
        esriSRGeoCS_Reunion = 4626,
        //
        // 摘要:
        //     Hjorsey 1955.
        esriSRGeoCS_Hjorsey1955 = 4658,
        //
        // 摘要:
        //     European 1979.
        esriSRGeoCS_European1979 = 4668,
        //
        // 摘要:
        //     LKS 1994.
        esriSRGeoCS_LKS1994 = 4669,
        //
        // 摘要:
        //     Chatham Island Astro 1971.
        esriSRGeoCS_ChathamIsland1971 = 4672,
        //
        // 摘要:
        //     Guam 1963.
        esriSRGeoCS_Guam1963 = 4675,
        //
        // 摘要:
        //     Gan 1970.
        esriSRGeoCS_Gan1970 = 4684,
        //
        // 摘要:
        //     Kerguelen Island 1949.
        esriSRGeoCS_KerguelenIsland1949 = 4698,
        //
        // 摘要:
        //     Tern Island Astro 1961.
        esriSRGeoCS_TernIsland1961 = 4707,
        //
        // 摘要:
        //     Anna 1 Astro 1965.
        esriSRGeoCS_Anna1_1965 = 4708,
        //
        // 摘要:
        //     Astro Beacon E 1945.
        esriSRGeoCS_BeaconE1945 = 4709,
        //
        // 摘要:
        //     Astro DOS 71/4.
        esriSRGeoCS_DOS71_4 = 4710,
        //
        // 摘要:
        //     Astronomical Station 1952.
        esriSRGeoCS_Astro1952 = 4711,
        //
        // 摘要:
        //     Ascension Island 1958.
        esriSRGeoCS_AscensionIsland1958 = 4712,
        //
        // 摘要:
        //     Ayabelle Lighthouse.
        esriSRGeoCS_Ayabelle = 4713,
        //
        // 摘要:
        //     Bellevue IGN.
        esriSRGeoCS_BellevueIGN = 4714,
        //
        // 摘要:
        //     Camp Area Astro.
        esriSRGeoCS_CampArea = 4715,
        //
        // 摘要:
        //     Canton Astro 1966.
        esriSRGeoCS_Canton1966 = 4716,
        //
        // 摘要:
        //     Cape Canaveral.
        esriSRGeoCS_CapeCanaveral = 4717,
        //
        // 摘要:
        //     Easter Island 1967.
        esriSRGeoCS_EasterIsland1967 = 4719,
        //
        // 摘要:
        //     ISTS 061 Astro 1968.
        esriSRGeoCS_ISTS061_1968 = 4722,
        //
        // 摘要:
        //     ISTS 073 Astro 1969.
        esriSRGeoCS_ISTS073_1969 = 4724,
        //
        // 摘要:
        //     Johnston Island 1961.
        esriSRGeoCS_JohnstonIsland1961 = 4725,
        //
        // 摘要:
        //     Midway Astro 1961.
        esriSRGeoCS_Midway1961 = 4727,
        //
        // 摘要:
        //     Pico de Las Nieves.
        esriSRGeoCS_PicodeLasNieves = 4728,
        //
        // 摘要:
        //     Pitcairn Astro 1967.
        esriSRGeoCS_Pitcairn1967 = 4729,
        //
        // 摘要:
        //     Santo DOS 1965.
        esriSRGeoCS_SantoDOS1965 = 4730,
        //
        // 摘要:
        //     Viti Levu 1916.
        esriSRGeoCS_VitiLevu1916 = 4731,
        //
        // 摘要:
        //     Wake-Eniwetok 1960.
        esriSRGeoCS_WakeEniwetok1960 = 4732,
        //
        // 摘要:
        //     Wake Island Astro 1952.
        esriSRGeoCS_WakeIsland1952 = 4733,
        //
        // 摘要:
        //     Tristan Astro 1968.
        esriSRGeoCS_Tristan1968 = 4734,
        //
        // 摘要:
        //     Kusaie Astro 1951.
        esriSRGeoCS_Kusaie1951 = 4735,
        //
        // 摘要:
        //     Deception Island.
        esriSRGeoCS_DeceptionIsland = 4736,
        //
        // 摘要:
        //     Hong Kong 1963.
        esriSRGeoCS_HongKong1963 = 4738,
        //
        // 摘要:
        //     WGS 1966.
        esriSRGeoCS_WGS1966 = 4760,
        //
        // 摘要:
        //     Bern 1898 (Bern).
        esriSRGeoCS_Bern1898Bern = 4801,
        //
        // 摘要:
        //     Bogota (Bogota).
        esriSRGeoCS_BogotaBogota = 4802,
        //
        // 摘要:
        //     Lisbon (Lisbon).
        esriSRGeoCS_LisbonLisbon = 4803,
        //
        // 摘要:
        //     Makassar (Jakarta).
        esriSRGeoCS_MakassarJakarta = 4804,
        //
        // 摘要:
        //     MGI (Ferro).
        esriSRGeoCS_MGIFerro = 4805,
        //
        // 摘要:
        //     Monte Mario (Rome).
        esriSRGeoCS_MonteMarioRome = 4806,
        //
        // 摘要:
        //     NTF (Paris).
        esriSRGeoCS_NTFParis = 4807,
        //
        // 摘要:
        //     Padang 1884 (Jakarta).
        esriSRGeoCS_Padang1884Jakarta = 4808,
        //
        // 摘要:
        //     Belge 1950 (Brussels).
        esriSRGeoCS_Belge1950Brussels = 4809,
        //
        // 摘要:
        //     Tananarive 1925 (Paris).
        esriSRGeoCS_Tananarive1925Paris = 4810,
        //
        // 摘要:
        //     Voirol 1875 (Paris).
        esriSRGeoCS_Voirol1875Paris = 4811,
        //
        // 摘要:
        //     Voirol Unifie 1960 (Paris).
        esriSRGeoCS_VoirolUnifie1960Paris = 4812,
        //
        // 摘要:
        //     Batavia (Jakarta).
        esriSRGeoCS_BataviaJakarta = 4813,
        //
        // 摘要:
        //     RT38 (Stockholm).
        esriSRGeoCS_RT38Stockholm = 4814,
        //
        // 摘要:
        //     Greek (Athens).
        esriSRGeoCS_GreekAthens = 4815,
        //
        // 摘要:
        //     Carthage (Paris).
        esriSRGeoCS_CarthageParis = 4816,
        //
        // 摘要:
        //     NGO 1948 (Oslo).
        esriSRGeoCS_NGO1948Oslo = 4817,
        //
        // 摘要:
        //     ATF (Paris).
        esriSRGeoCS_ATFParis = 4901,
        //
        // 摘要:
        //     Nord de Guerre (Paris).
        esriSRGeoCS_NorddeGuerreParis = 4902,
        //
        // 摘要:
        //     Madrid 1870 (Madrid).
        esriSRGeoCS_Madrid1870Madrid = 4903,
        //
        // 摘要:
        //     Fischer 1960.
        esriSRGeoCS_Fischer1960 = 37002,
        //
        // 摘要:
        //     Fischer 1968.
        esriSRGeoCS_Fischer1968 = 37003,
        //
        // 摘要:
        //     Fischer modified.
        esriSRGeoCS_ModifiedFischer = 37004,
        //
        // 摘要:
        //     Hough 1960.
        esriSRGeoCS_Hough1960 = 37005,
        //
        // 摘要:
        //     Everest modified 1969.
        esriSRGeoCS_ModifiedEverest1969 = 37006,
        //
        // 摘要:
        //     Walbeck.
        esriSRGeoCS_Walbeck = 37007,
        //
        // 摘要:
        //     Authalic sphere (ARC/INFO).
        esriSRGeoCS_AuthalicsphereARCINFO = 37008,
        //
        // 摘要:
        //     Everest - Bangladesh.
        esriSRGeoCS_EverestBangladesh = 37202,
        //
        // 摘要:
        //     Everest - India and Nepal.
        esriSRGeoCS_EverestIndiaNepal = 37203,
        //
        // 摘要:
        //     Oman.
        esriSRGeoCS_Oman = 37206,
        //
        // 摘要:
        //     South Asia Singapore.
        esriSRGeoCS_SouthAsiaSingapore = 37207,
        //
        // 摘要:
        //     DOS 1968.
        esriSRGeoCS_DOS1968 = 37218,
        //
        // 摘要:
        //     GUX 1 Astro.
        esriSRGeoCS_GUX1 = 37221,
        //
        // 摘要:
        //     Fort Thomas 1955.
        esriSRGeoCS_FortThomas1955 = 37240,
        //
        // 摘要:
        //     Graciosa Base SW 1948.
        esriSRGeoCS_Graciosa1948 = 37241,
        //
        // 摘要:
        //     L.C. 5 Astro 1961.
        esriSRGeoCS_LC5_1961 = 37243,
        //
        // 摘要:
        //     Observ. Meteorologico 1939.
        esriSRGeoCS_ObservMeteor1939 = 37245,
        //
        // 摘要:
        //     Sao Braz.
        esriSRGeoCS_SaoBraz = 37249,
        //
        // 摘要:
        //     S-42 Hungary.
        esriSRGeoCS_S_42Hungary = 37257,
        //
        // 摘要:
        //     Alaskan Islands.
        esriSRGeoCS_AlaskanIslands = 37260,
        //
        // 摘要:
        //     Assumed Geographic 1.
        esriSRGeoCS_AssumedGeographic1 = 104000,
        //
        // 摘要:
        //     International 1967.
        esriSRGeoCS_International1967 = 104023,
        //
        // 摘要:
        //     European Terrestrial Ref. Frame 1989.
        esriSRGeoCS_ETRF1989 = 104258,
        //
        // 摘要:
        //     Merchich (Degree).
        esriSRGeoCS_MerchichDegree = 104261,
        //
        // 摘要:
        //     Voirol Unifie 1960 (Degree).
        esriSRGeoCS_VoirolUnifie1960Degree = 104305
    }
}
