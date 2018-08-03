﻿/*
Deployment script for OpenHousePortals

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "OpenHousePortals"
:setvar DefaultFilePrefix "OpenHousePortals"
:setvar DefaultDataPath "D:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "D:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering [SQLPortalFunctionsKFH]...';


GO
ALTER ASSEMBLY [SQLPortalFunctionsKFH]
    DROP FILE ALL;


GO
ALTER ASSEMBLY [SQLPortalFunctionsKFH]
    FROM 0x4D5A90000300000004000000FFFF0000B800000000000000400000000000000000000000000000000000000000000000000000000000000000000000800000000E1FBA0E00B409CD21B8014CCD21546869732070726F6772616D2063616E6E6F742062652072756E20696E20444F53206D6F64652E0D0D0A2400000000000000504500004C010300CC7F3B5B0000000000000000E00002210B010B00003E00000008000000000000AE5D000000200000006000000000001000200000000200000400000000000000040000000000000000A00000000200006B450100030040850000100000100000000010000010000000000000100000000000000000000000585D000053000000006000005804000000000000000000000000000000000000008000000C00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200000080000000000000000000000082000004800000000000000000000002E74657874000000B43D000000200000003E000000020000000000000000000000000000200000602E7273726300000058040000006000000006000000400000000000000000000000000000400000402E72656C6F6300000C0000000080000000020000004600000000000000000000000000004000004200000000000000000000000000000000905D000000000000480000000200050068340000F028000009000000000000000000000000000000502000008000000000000000000000000000000000000000000000000000000000000000000000008801C005939E132A1F2400AB82ECCEB873CF826B7F30767CB43755C27687DF2F0A74DF57708A1D4F0011409C61FFA08B6413E6F50CAA6A8FD37D9C11A809590E3125838A353BA4CC72D870C349622F5ED2F72D7EAA93BB5C18BA8FD6EC228173A4ACB72A9B709D90B404E53137690ED360786027B304FB38D488797CD092DD8913300A005E010000010000117E0F00000A0A036F1000000A6F1100000AD01E000001281200000A403D010000036F1000000A741E0000010B076F1300000A74200000010C036F1400000A72010000706F1500000A2C13086F1600000A74040000026F060000062D012A086F1600000A74040000026F080000060D036F1700000A13051105450200000001000000010000002A096F1800000A16316509166F1900000A740500000213041104722F00007011046F130000068C05000001036F1400000A281A00000A281B00000A6F14000006086F1600000A74040000026F08000006166F1C00000A086F1600000A74040000026F0800000611046F1D00000A262A7241000070036F1400000A281E00000A0A086F1600000A74040000026F0800000614281B00000A1416281F00000A17281F00000A06281B00000A728B000070281B00000A14281B00000A1206FE150100001B11061207FE150100001B1107731D0000066F1D00000A262A00001B300A00E50D000002000011732100000A0A282200000A282300000A0B140C140D0F00282400000A2D1C0F00282500000A72C1000070282600000A2D090F01282400000A2C4F0614281B00000A1416281F00000A17281F00000A72C3000070281B00000A7233010070281B00000A14281B00000A1229FE150100001B1129122AFE150100001B112A731D0000066F1D00000A26062A0F01282500000A7257010070282700000A2C620F01282500000A7261010070282700000A2C4F0614281B00000A1416281F00000A17281F00000A7269010070281B00000A7233010070281B00000A14281B00000A122BFE150100001B112B122CFE150100001B112C731D0000066F1D00000A26062A0F00282500000A161202282800000A2D4F0614281B00000A1416281F00000A17281F00000A72CD010070281B00000A7233010070281B00000A14281B00000A122DFE150100001B112D122EFE150100001B112E731D0000066F1D00000A26062A0F07282400000A2D57056F2900000A2C4F0614281B00000A1416281F00000A17281F00000A7229020070281B00000A7233010070281B00000A14281B00000A122FFE150100001B112F1230FE150100001B1130731D0000066F1D00000A26062A0F07282400000A2D5D0F07282500000A282A00000A2D4F0614281B00000A1416281F00000A17281F00000A7295020070281B00000A7233010070281B00000A14281B00000A1231FE150100001B11311232FE150100001B1132731D0000066F1D00000A26062A0F08282400000A2C120F09282400000A2C090F0A282400000A2D57056F2900000A2C4F0614281B00000A1416281F00000A17281F00000A7240030070281B00000A7233010070281B00000A14281B00000A1233FE150100001B11331234FE150100001B1134731D0000066F1D00000A26062A0F05282400000A2D580F04282400000A2C4F0614281B00000A1416281F00000A17281F00000A7205040070281B00000A7233010070281B00000A14281B00000A1235FE150100001B11351236FE150100001B1136731D0000066F1D00000A26062A0F04282400000A2D5D0F04282500000A282A00000A2D4F0614281B00000A1416281F00000A17281F00000A7290040070281B00000A7233010070281B00000A14281B00000A1237FE150100001B11371238FE150100001B1138731D0000066F1D00000A26062A0F04282400000A3A9F0000000F05282400000A2D160F04282500000A0F05282500000A732B00000A262B0D0F04282500000A732C00000A26DE7113040614281B00000A1416281F00000A17281F00000A724105007011046F2D00000A11046F2E00000A11046F2F00000A283000000A281B00000A7233010070281B00000A14281B00000A1239FE150100001B1139123AFE150100001B113A731D0000066F1D00000A26061328DDDA090000733100000A1305056F2900000A3AD4010000730A00000613060F08282400000A2D43730A00000613071107056F3200000A6F3300000A11060F08282500000A6F3400000A1308110611086F3500000A26110811076F3600000A6F3700000A6F3800000A2B0D1106056F3200000A6F3300000A11066F3900000A6F3A00000A1F112E291106728D0500707295050070146F3B00000A130911066F3600000A130A11061109110A6F3C00000A260F07282400000A2D53733D00000A130B110B72C10000700F07282500000A6F3E00000A2611066F3F00000A110B6F4000000A0F0B284100000A2D0E11060F0B284200000A6F07000006110614FE0601000006734300000A6F4400000A11066F080000066F1800000A1631150611066F080000066F4500000A061328DDAF0800001105284600000A130C1106110C6F4700000A110C6F4800000ADE0C110C2C07110C6F4900000ADC734A00000A130D110D11066F3700000A6F4B00000A0DDE7C130E72A1050070110E6F2D00000A110E6F2E00000A110E6F2F00000A283000000A130F0611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A110F281B00000A72E9050070281B00000A14281B00000A07734E00000A123BFE150100001B113B731D0000066F1D00000A26061328DDF40700000F06284F00000A2C091F14285000000A10060F02282400000A2C0C72FD050070281B00000A100208285100000A7437000001131011100F01282500000A6F5200000A11100F02282500000A6F5300000A11100F02282500000A6F5400000A11100F06285500000A20B80B00005A6F5600000A1110176F5700000A1110176F5800000ADE7C1311720506007011116F2D00000A11116F2E00000A11116F2F00000A283000000A13120611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1112281B00000A725D060070281B00000A14281B00000A07734E00000A123CFE150100001B113C731D0000066F1D00000A26061328DDF40600000F04282400000A2D440F05282400000A2D2211106F5900000A0F04282500000A0F05282500000A732B00000A6F5A00000A262B1911106F5900000A0F04282500000A732C00000A6F5A00000A26DE7C1313724105007011136F2D00000A11136F2E00000A11136F2F00000A283000000A13140611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1114281B00000A726D060070281B00000A14281B00000A07734E00000A1229FE150100001B1129731D0000066F1D00000A26061328DD290600000F01282500000A7257010070282600000A395301000011106F5B00000A285C00000A735D00000A1315092C191115285C00000A096F5E00000A6F5F00000A11156F6000000ADD810000001316729106007011166F2D00000A11166F2E00000A11166F2F00000A283000000A13170611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1117281B00000A72DF060070281B00000A14281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26061328DD5E050000DE0811156F6100000ADCDE0C11152C0711156F4900000ADCDD810000001318720507007011186F2D00000A11186F2E00000A11186F2F00000A283000000A13190611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1119281B00000A72DF060070281B00000A14281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26061328DDC00400007E0F00000A131A14131B11106F6200000A743D000001131B11106F6300000A39FE030000111B6F6400000A736500000A131C111C6F6600000A131A14131D111A7251070070166F6700000A183018736800000A131D111D111A6F3300000ADE062614131DDE00111D39B90200000F09282400000A3A330200000F0A282400000A3A27020000111D0F09282500000A6F6900000A131E111D736A00000A131F111E6F6B00000A2D680611056F4C00000A6F4D00000A281B00000A111F736C00000A16281F00000A16281F00000A14281B00000A7255070070281B00000A0E0C121D2803000006281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A263801010000111E6F6D00000A132038E700000011206F6E00000A74210000016F6F00000A0F0A282500000A282600000A2C650611056F4C00000A6F4D00000A281B00000A111F736C00000A17281F00000A16281F00000A14281B00000A7291070070281B00000A0E0C121D2803000006281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A262B630611056F4C00000A6F4D00000A281B00000A111F736C00000A16281F00000A17281F00000A14281B00000A725D060070281B00000A0E0C121D2803000006281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A2611206F7000000A3A0DFFFFFFDE0C111F2C07111F6F4900000ADCDD61010000132172A107007011216F2D00000A11216F2E00000A11216F2F00000A283000000A13220611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1122281B00000A725D060070281B00000A0E0C121D2803000006281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26061328DD14020000111D736A00000A13230611056F4C00000A6F4D00000A281B00000A1123736C00000A17281F00000A16281F00000A14281B00000A7291070070281B00000A0E0C121D2803000006281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26DE6A11232C0711236F4900000ADC0611056F4C00000A6F4D00000A281B00000A1417281F00000A16281F00000A111A281B00000A7291070070281B00000A0E0C121D2803000006281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26DD810000001324729106007011246F2D00000A11246F2E00000A11246F2F00000A283000000A13250611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1125281B00000A725D060070281B00000A14281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26061328DDB6000000DE08111C6F7100000ADCDE0C111C2C07111C6F4900000ADCDE7E132672EB07007011266F2D00000A11266F2E00000A11266F2F00000A283000000A13270611056F4C00000A6F4D00000A281B00000A1416281F00000A17281F00000A1127281B00000A725D060070281B00000A14281B00000A07734E00000A282200000A282300000A734E00000A731D0000066F1D00000A26061328DE1EDE0C111B2C07111B6F7200000ADCDE0C11052C0711056F4900000ADC062A11282A00000041CC010000000000690300002E00000097030000710000001C000001020000003C050000120000004E0500000C00000000000000000000002104000051010000720500007C0000001C00000100000000150600005D000000720600007C0000001C00000100000000EE0600004F0000003D0700007C0000001C00000100000000E20700002100000003080000810000001C00000102000000E2070000A400000086080000080000000000000002000000E2070000AE000000900800000C0000000000000000000000CF070000D2000000A1080000810000001C00000100000000770900000B00000082090000060000001C00000102000000C009000074010000340B00000C0000000000000000000000A70900009E010000450B0000890000001C00000102000000D70B0000650000003C0C00000C00000000000000000000005409000057030000AB0C0000810000001C0000010200000054090000DA0300002E0D000008000000000000000200000054090000E4030000380D00000C00000000000000000000002C0900001A040000460D00007E0000001C000001020000002C0900009A040000C60D00000C00000000000000020000000F040000C5090000D40D00000C000000000000001B30030060000000030000110F00282400000A2D5303500F00282500000A6F6900000A0A066F6B00000A2D04140DDE3A066F6D00000A0B140C2B1C08076F6E00000A74210000016F6F00000A7239080070287300000A0C076F7000000A2DDC080DDE0726140DDE02142A092A01100000000009004E5700051C0000011330020076000000040000110274050000020A03066F0B000006810500000104066F0D0000065105066F0F00000681080000010E04066F1100000681080000010E05066F1300000681050000010E06066F1900000681090000010E07066F1B00000681090000010E08066F1500000681050000010E09066F1700000681050000012A1E02287400000A2A1E027B110000042A2202037D110000042A1E027B120000042A2202037D120000042A4A02732100000A7D1200000402286800000A2A1E027B130000042A2202037D130000042A1E027B140000042A2202037D140000042A1E027B150000042A2202037D150000042A1E027B160000042A2202037D160000042A1E027B170000042A2202037D170000042A1E027B180000042A2202037D180000042A1E027B190000042A2202037D190000042A1E027B1A0000042A2202037D1A0000042A1E027B1B0000042A2202037D1B0000042A033002008B0000000000000002287400000A0203280C0000060204280E00000602052810000006020E042812000006020E052814000006020E062816000006020E0728180000060F08287500000A2D1202282200000A282300000A281A0000062B0D020F08287600000A281A0000060F09287500000A2D1102282200000A282300000A281C0000062A020F09287600000A281C0000062A0042534A4201000100000000000C00000076322E302E35303732370000000005006C000000540A0000237E0000C00A0000D80C000023537472696E677300000000981700004008000023555300D81F0000100000002347554944000000E81F00000809000023426C6F620000000000000002000001571DA2090902000000FA2533001600000100000043000000050000001B0000001D0000002F00000076000000120000000C00000004000000020000000B000000160000000100000001000000040000000300000000000A00010000000000060066005F000A0078006D000A00960084000600D400C1000E000101EC000E000B01EC000E001201EC000E001B01EC000E005101EC000600B702C100060075045F000600930574050600B805A6050600CF05A6050600EC05A60506000B06A60506002406A60506003D06A60506005806A60506007306A60506008C0674050600A006A6050600B906A6050600F606D60606001607D60606004A075F000A0057078400060078075F00060082075F000A008F0784000600AC075F000A00E1076D000A0001086D000A001B0884000E0086086B0806009B085F001200DB085F001200DF085F000600FB08F10812003509070906006009F1080A00CC096D000A00E5096D000A00160A84000A00230A84000A00AA0084000600420AC1000A00570A6D000600610AF1080600810A5F000600A10A950A0600AE0A950A0600C00A950A1200F30AE80A1200FE0AE80A12005D0B070906008E0B070906009E0BF1080600BF0BF1081200E20BE80A1200FA0BE80A06002D0CF10806003A0CF1080A00570C6D000A006F0C6D000A007D0C6D000600870CC100000000000100000000000100010001001000240000000500010001008201100031000000050001000600030010003E000000090011000600030010004B000000050013000B0056806B015D0056807F015D00568091015D0056809E015D005680A8015D005680BB015D005680D6015D005680E0015D005680F3015D00568007025D0056800F025D00568017025D00568035025D00568047025D0056805B025D00568067025D0001007B0270020100C1027C0201001F03940201004603A30201007303B20201009403B2020100B50394020100EB03940201000304940201003004C10201005104C102D020000000009100AA000A0001003C22000000009600260111000300FC310000000091003B013000100078320000000096005D0139001200FA32000000008618650159001C0002330000000086088D0273021C000A33000000008608A20277021C001333000000008608D30280021D001B33000000008608E80285021D002433000000008618650159001E0037330000000086082A0398021E003F3300000000860838039D021E0048330000000086085303A7021F0050330000000086086303AC021F0059330000000086087C03B602200061330000000086088803BB0220006A330000000086089D03B60221007233000000008608A903BB0221007B33000000008608C503980222008333000000008608D8039D0222008C33000000008608F103980223009433000000008608FA039D0223009D33000000008608100498022400A53300000000860820049D022400AE330000000086083904C5022500B6330000000086084504CA022500BF330000000086085B04C5022600C7330000000086086804CA022600D0330000000086186501D002270000000100D70400000200DE0400000100E00400000200E80400000300EF0400000400FB04000005000305000006000F05000007001B05000008002305000009002D0500000A00360500000B00420500000C00FD0200000D004F05000001006005000002006305000001006605020002008004020003008A04020004009604020005009E0402000600A60402000700C60402000800CE0402000900B50402000A00BA0400000100A00500000100A00500000100A00500000100A00500000100A00500000100A00500000100A00500000100A00500000100A00500000100A00500000100A005000001008004000002008A04000003009604000004009E0400000500A60400000600B50400000700BA0410100800C60410100900CE04610065015900690065010703710065010703790065010703810065010703890065010703910065010703990065010703A10065010703A90065017702B10065010703B90065010703C10065010C03C90065015900D10051075D0019006A07B303E1008707B803E900BE07BD03F100D007C4031900EC07C803D100F807CC0309010908D10319002B08D60351003808DC0351004208E003D1004B08E50329005208EC0351005E080C0351006708F203D1004B08F70341005208FD031901650159005100650159002101A40811064900520817062900AC0873022900B708C803D100C1081F06D100CD081F062901E70825063100AC087302390100093106410165013606410165010703E1004609C803E100EC07C803E1005109C803D1004B083C064901650159003100B708C80311006D090703110075094406090183094B0611008F0954060901A309C8030901B00907030901BD095A060901D80960061100F40966060901090A6F06610165015900610167087B0611002D0A83066101670889064100AC0873024100B70873027101650190061100390A960651004E0A9D0681016C0AA4060901730AAD0681017B0A590091018D0A5900990165015900A101B70AB4064901CE0ABA060900DF0AC8030C006501C0063900AC08730239005208C606B1016C0ACC06B1010D0B0703B101180B0703B901280B07033900B708DC03B101330B0C03B9013F0B7702B9014F0B7702B901770BD506C1016708DB06B101A50BE206A101B60BE806D9016501EE06A101CC0BF8068901D60B070389017B0A59008901DC0B5900B101EE0BFE06B9010A0C7302E1011B0CE206F10165010407F901450CC803D1004F0C0B071100650159000901630C110709026501180701023808DC03310065011F070102930C26071902A10CC4030901AD0CC8031902BB0C7302F901DC0B5900E101DC0B5900D100C40C01080900650159000C00CB0C73020C00B70818080E00040060000E00080087000E000C00AA000E001000C3000E001400D6000E001800FB000E001C0030010E00200043010E00240068010E0028008F010E002C009E010E003000AD010E003400E8010E0038000B020E003C0032020E00400049021200B90002031200BD0002032E003300A0082E0013001D082E001B0040082E0023007A082E002B0080082E0053007A082E007300E9082E0043007A082E003B00BB082E006300D3082E006B00E0084000030126040A042C070808130804000100050003000000FD028B0200000E038F0200008004EE0200008A04F30200009604F80200009E04F8020000A604EE020000B504EE020000BA04EE020000C604FD020000CE04FD0202000600030001000700030002000800050001000900050002000B00070001000C00070001000E00090002000D00090002000F000B00010010000B00020011000D00010012000D00010014000F00020013000F0002001500110001001600110002001700130001001800130001001A00150002001900150002001B00170001001C001700030404800000010001000000000001000000110334070000020000000000000000000000010056000000000002000000000000000000000001006D00000000000200000000000000000000000100E0000000000002000000000000000000000001005F00000000000300020004000200050002000000003C4D6F64756C653E0053514C506F7274616C46756E6374696F6E734B46482E646C6C0046756E6374696F6E734B4648005354415455535F434F44455300586D6C446F63756D656E743200506F7374526573756C74006D73636F726C69620053797374656D004F626A6563740053797374656D2E586D6C00586D6C446F63756D656E740053797374656D2E586D6C2E536368656D610056616C69646174696F6E4576656E74417267730056616C69646174696F6E4576656E7448616E646C65720053797374656D2E436F6C6C656374696F6E730049456E756D657261626C650053797374656D2E446174610053797374656D2E446174612E53716C54797065730053716C537472696E670053716C586D6C0053716C496E7433320053716C426F6F6C65616E00666E57656253656E64584D4C44617461546573740052657475726E537472696E6746726F6D58506174680053716C4461746554696D650046696C6C526F77002E63746F720043455254494649434154455F4D495353494E470043455254494649434154455F4552524F5200584D4C5F455850454354454400584D4C5F4552524F5200584D4C5F534348454D415F4D495353494E4700584D4C5F494E56414C49445F414741494E53545F534348454D410053514C5F4552524F52004745545F524551554553545F53545245414D004745545F524553504F4E53455F53545245414D0053554343455353004641494C55524500584D4C5F58504154485F524553554C545F4E4F44455F4D495353494E47004641494C45445F56414C49444154494F4E0046494C455F4558495354535F534B49505045440046494C455F4558495354530046494C455F444F45535F4E4F545F4558495354005F556E697175654E6F646556616C756573006765745F556E697175654E6F646556616C756573007365745F556E697175654E6F646556616C7565730041727261794C697374005F56616C69646174696F6E4572726F7273006765745F56616C69646174696F6E4572726F7273007365745F56616C69646174696F6E4572726F727300556E697175654E6F646556616C7565730056616C69646174696F6E4572726F7273005F584D4C506F73746564006765745F584D4C506F73746564007365745F584D4C506F73746564005F584D4C526573706F6E7365006765745F584D4C526573706F6E7365007365745F584D4C526573706F6E7365005F53756363657373006765745F53756363657373007365745F53756363657373005F4661696C757265006765745F4661696C757265007365745F4661696C757265005F4164646974696F6E616C496E666F006765745F4164646974696F6E616C496E666F007365745F4164646974696F6E616C496E666F005F436F6465006765745F436F6465007365745F436F6465005F52657475726E56616C7565006765745F52657475726E56616C7565007365745F52657475726E56616C7565005F53746172746564006765745F53746172746564007365745F53746172746564005F46696E6973686564006765745F46696E6973686564007365745F46696E6973686564004E756C6C61626C65603100584D4C506F7374656400584D4C526573706F6E73650053756363657373004661696C757265004164646974696F6E616C496E666F00436F64650052657475726E56616C756500537461727465640046696E69736865640073656E646572006500506F737455524C004D6574686F6400436F6E74656E745479706500584D4C446174610050313243657274506174680050313243657274506173730054696D656F757400536368656D6155524900526F6F744E616D6500537563636573734E6F6465005375636365737356616C756500585061746852657475726E56616C756500787000786400706F7374526573756C744F626A0053797374656D2E52756E74696D652E496E7465726F705365727669636573004F75744174747269627574650076616C75650053797374656D2E5265666C656374696F6E00417373656D626C795469746C6541747472696275746500417373656D626C794465736372697074696F6E41747472696275746500417373656D626C79436F6E66696775726174696F6E41747472696275746500417373656D626C79436F6D70616E7941747472696275746500417373656D626C7950726F6475637441747472696275746500417373656D626C79436F7079726967687441747472696275746500417373656D626C7954726164656D61726B41747472696275746500417373656D626C7943756C7475726541747472696275746500436F6D56697369626C6541747472696275746500417373656D626C7956657273696F6E41747472696275746500417373656D626C7946696C6556657273696F6E4174747269627574650053797374656D2E52756E74696D652E436F6D70696C6572536572766963657300436F6D70696C6174696F6E52656C61786174696F6E734174747269627574650052756E74696D65436F6D7061746962696C6974794174747269627574650053514C506F7274616C46756E6374696F6E734B464800537472696E6700456D70747900586D6C536368656D61457863657074696F6E006765745F457863657074696F6E00457863657074696F6E0054797065004765745479706500586D6C536368656D6156616C69646174696F6E457863657074696F6E0052756E74696D655479706548616E646C65004765745479706546726F6D48616E646C65006765745F536F757263654F626A65637400586D6C456C656D656E74006765745F4D65737361676500436F6E7461696E7300586D6C4E6F6465006765745F4F776E6572446F63756D656E7400586D6C536576657269747954797065006765745F5365766572697479006765745F436F756E74006765745F4974656D00466F726D6174006F705F496D706C696369740052656D6F7665417400416464004D6963726F736F66742E53716C5365727665722E5365727665720053716C46756E6374696F6E417474726962757465004461746554696D65006765745F4E6F77006765745F49734E756C6C006765745F56616C7565006F705F457175616C697479006F705F496E657175616C69747900557269005572694B696E64005472794372656174650053797374656D2E494F0046696C65004578697374730053797374656D2E53656375726974792E43727970746F6772617068792E583530394365727469666963617465730058353039436572746966696361746532006765745F536F75726365006765745F537461636B547261636500537472696E67577269746572004C6F6164586D6C00437265617465456C656D656E7400417070656E644368696C64006765745F446F63756D656E74456C656D656E74006765745F496E6E6572586D6C007365745F496E6E6572586D6C006765745F46697273744368696C6400586D6C4E6F646554797065006765745F4E6F64655479706500586D6C4465636C61726174696F6E00437265617465586D6C4465636C61726174696F6E00496E736572744265666F726500586D6C536368656D6153657400586D6C536368656D61006765745F536368656D61730056616C69646174650049436F6C6C656374696F6E0041646452616E676500586D6C577269746572005465787457726974657200437265617465005772697465546F00466C7573680049446973706F7361626C6500446973706F73650053797374656D2E546578740055544638456E636F64696E6700456E636F64696E6700476574427974657300537472696E674275696C64657200476574537472696E674275696C64657200546F537472696E670053797374656D2E4E65740057656252657175657374004874747057656252657175657374007365745F4D6574686F64007365745F436F6E74656E7454797065007365745F416363657074007365745F54696D656F7574007365745F53656E644368756E6B6564007365745F4B656570416C69766500583530394365727469666963617465436F6C6C656374696F6E006765745F436C69656E74436572746966696361746573005835303943657274696669636174650053747265616D004765745265717565737453747265616D006765745F555446380053747265616D57726974657200476574537472696E6700577269746500436C6F736500576562526573706F6E736500476574526573706F6E73650048747470576562526573706F6E7365006765745F48617665526573706F6E736500476574526573706F6E736553747265616D0053747265616D52656164657200546578745265616465720052656164546F456E6400496E6465784F6600586D6C4E6F64654C6973740053656C6563744E6F64657300586D6C4E6F646552656164657200586D6C5265616465720049456E756D657261746F7200476574456E756D657261746F72006765745F43757272656E74006765745F496E6E657254657874004D6F76654E65787400436F6E636174006765745F48617356616C756500002D6400750070006C006900630061007400650020006B00650079002000730065007100750065006E006300650000117B0030007D000D000A007B0031007D00004958004D004C00200053006300680065006D0061002000760061006C00690064006100740069006F006E0020006500720072006F0072002800730029003A000D000A007B0030007D00003558004D004C005F0049004E00560041004C00490044005F0041004700410049004E00530054005F0053004300480045004D0041000001006F54006800650020005B0050006F00730074002000550052004C005D00200077006100730020006D0069007300730069006E00670020006F007200200074006800650020005B004D006500740068006F0064005D00200077006100730020006D0069007300730069006E0067002E0000234600410049004C00450044005F00560041004C00490044004100540049004F004E00000950004F00530054000007470045005400006354006800650020005B004D006500740068006F0064005D002000700072006F0076006900640065006400200077006100730020006E006F0074002000650069007400680065007200200050004F005300540020006F00720020004700450054002E00005B54006800650020005B00550052004C005D002000700072006F00760069006400650064002000770061007300200069006E0063006F00720072006500630074006C007900200066006F0072006D00610074007400650064002E00006B54006800650020005B0053006300680065006D00610020005500520049005D0020007700610073002000700072006F0076006900640065006400200062007500740020005B0058004D004C00200044006100740061005D00200077006100730020006E006F0074002E000080A954006800650020005B0053006300680065006D00610020005500520049005D0020007700610073002000700072006F00760069006400650064002000620075007400200063006F0075006C00640020006E006F00740020006200650020006C006F00610064006500640020006200650063006100750073006500200069007400200063006F0075006C00640020006E006F007400200062006500200066006F0075006E0064002E000080C354006800650020005B0052006F006F00740020004E0061006D0065005D0020006F00720020005B00530075006300630065007300730020004E006F00640065005D0020006F00720020005B0053007500630063006500730073002000560061006C00750065005D0020007700610073002000700072006F00760069006400650064002000620075007400200074006800650020005B0058004D004C00200044006100740061005D00200077006100730020006D0069007300730069006E0067002E0000808954006800650020005B0050003100320020004300650072007400200050006100730073005D0020007700610073002000700072006F00760069006400650064002000620075007400200074006800650020005B0050003100320020004300650072007400200050006100740068005D0020006900730020006D0069007300730069006E0067002E000080AF54006800650020005B0050003100320020004300650072007400200050006100740068005D0020007700610073002000700072006F00760069006400650064002000620075007400200063006F0075006C00640020006E006F00740020006200650020006C006F00610064006500640020006200650063006100750073006500200069007400200063006F0075006C00640020006E006F007400200062006500200066006F0075006E0064002E00004B43006500720074006900660069006300610074006500200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E00000731002E003000000B5500540046002D003800014758004D004C00200050006100720073006500200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E00001358004D004C005F004500520052004F00520000072A002F002A00005757006500620052006500710075006500730074002000430072006500610074006500200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E00000F4600410049004C005500520045000023430045005200540049004600490043004100540045005F004500520052004F005200004D530074007200650061006D00570072006900740065007200200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E0000254700450054005F0052004500510055004500530054005F00530054005200450041004D00004B57006500620020007200650071007500650073007400200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E0000033C00003B58004D004C005F00580050004100540048005F0052004500530055004C0054005F004E004F00440045005F004D0049005300530049004E004700000F5300550043004300450053005300004958004D004C002000720065006100640065007200200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E00004D570065006200200072006500730070006F006E0073006500200065007800630065007000740069006F006E002E0020007B0030007D002E0020007B0031007D002E0020007B0032007D002E0000050D000A00000087FE0F9E929FDC4099C8F584AFA4591C0008B77A5C561934E089060002011C120D1E000D1211111511151115121911151115111D1115111511151115112111150800020E11151012091F000A011C1011151012191011211011211011151011251011251011151011150320000102060E26430045005200540049004600490043004100540045005F004D0049005300530049004E00470022430045005200540049004600490043004100540045005F004500520052004F0052001858004D004C005F00450058005000450043005400450044001258004D004C005F004500520052004F0052002458004D004C005F0053004300480045004D0041005F004D0049005300530049004E0047003458004D004C005F0049004E00560041004C00490044005F0041004700410049004E00530054005F0053004300480045004D00410012530051004C005F004500520052004F005200244700450054005F0052004500510055004500530054005F00530054005200450041004D00264700450054005F0052004500530050004F004E00530045005F00530054005200450041004D000E53005500430043004500530053000E4600410049004C005500520045003A58004D004C005F00580050004100540048005F0052004500530055004C0054005F004E004F00440045005F004D0049005300530049004E004700224600410049004C00450044005F00560041004C00490044004100540049004F004E0026460049004C0045005F004500580049005300540053005F0053004B004900500050004500440016460049004C0045005F0045005800490053005400530026460049004C0045005F0044004F00450053005F004E004F0054005F00450058004900530054000206020320000204200101020306122904200012290520010112290328000204280012290306111504200011150520010111150306121904200012190520010112190306112104200011210520010111210306112504200011250520010111251D200901111512191121112111151115111515112D01112515112D01112504280011150428001219042800112104280011250400000000042001010E042001010880A00024000004800000940000000602000000240000525341310004000001000100F1169D878D3C9A207FCFB675A6FBF577D43C38B95B699DEE10DE01CE8B1D61A8DC16D04D30CAC13496A3F57F867EE93BD9CCBAA8F141ACB204A85BC933087306F7379C17F40841B3013A7BB0655543D5E9AD8E87F500B3143CD2EB21BABB15DE5B6B71D985F9E5187A8BA390060A2A3B56DE06102E9F78E042E749E6C56747B8042000126D04200012750600011275117D0320001C0320000E042001020E0420001209052000118089032000080420011C080600030E0E1C1C05000111150E042001081C0500020E0E1C0500011121020615112D0111251B07080E12791280811229121411808915112D01112515112D01112581E901000300540E1146696C6C526F774D6574686F644E616D650746696C6C526F775455794D6963726F736F66742E53716C5365727665722E5365727665722E446174614163636573734B696E642C2053797374656D2E446174612C2056657273696F6E3D322E302E302E302C2043756C747572653D6E65757472616C2C205075626C69634B6579546F6B656E3D623737613563353631393334653038390A4461746141636365737301000000540E0F5461626C65446566696E6974696F6E812A0D0A202020202020202020202020584D4C506F73746564206E76617263686172286D6178292C0D0A202020202020202020202020584D4C526573706F6E736520786D6C2C0D0A20202020202020202020202053756363657373206269742C0D0A2020202020202020202020204661696C757265206269742C0D0A2020202020202020202020204164646974696F6E616C496E666F206E76617263686172286D6178292C0D0A20202020202020202020202053746172746564206461746574696D652C0D0A20202020202020202020202046696E6973686564206461746574696D652C0D0A202020202020202020202020436F6465206E7661726368617228313030292C0D0A20202020202020202020202052657475726E56616C7565206E76617263686172286D6178290500001180910700011125118091050002020E0E0B0003020E11809910128095040001020E052002010E0E0700040E0E1C1C1C0620011280810E0820011280851280850520001280810520001280850520001180A90820031280AD0E0E0E0B20021280851280851280850720021280B50E0E0520001280B1062001011280B1052002011C18062001011280B9062001011280BD0800011280C11280C5062001011280C10520011D050E0520001280D5052001011300050001111D080800011280D91280950520001280E1062001081280E50520001280E90500001280D1092002011280E91280D10520010E1D050520001280F1062001011280E9052002080E080620011281010E062001011280850620010112810905200012810D80D3073D122911251280951D0512711280A5121012101280811280AD1280811280B11280C11280CD12710E1280DD12710E12710E1280ED12710E12710E0E1280F51280F9120912810112810512810D12710E12810512710E12710E121115112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D01112515112D0111250600030E0E0E0E0A070412810112810D0E0E040701121404200013002201001D4323205765622046756E6374696F6E7320434C5220417373656D626C790000390100345573656420746F204F70656E486F75736520746F207570646174652077656220706F7274616C7320696E207265616C2074696D6500000501000000001F01001A4B696E6C6569676820466F6C6B6172642026204861797761726400001A01001553514C506F7274616C46756E6374696F6E734B4648000017010012436F7079726967687420C2A920203230313700000C010007312E312E302E3000000801000800000000001E01000100540216577261704E6F6E457863657074696F6E5468726F777301805D000000000000000000009E5D0000002000000000000000000000000000000000000000000000905D000000000000000000000000000000005F436F72446C6C4D61696E006D73636F7265652E646C6C0000000000FF25002000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000100100000001800008000000000000000000000000000000100010000003000008000000000000000000000000000000100000000004800000058600000FC0300000000000000000000FC0334000000560053005F00560045005200530049004F004E005F0049004E0046004F0000000000BD04EFFE00000100010001000000000001000100000000003F000000000000000400000002000000000000000000000000000000440000000100560061007200460069006C00650049006E0066006F00000000002400040000005400720061006E0073006C006100740069006F006E00000000000000B0045C030000010053007400720069006E006700460069006C00650049006E0066006F00000038030000010030003000300030003000340062003000000084003500010043006F006D006D0065006E007400730000005500730065006400200074006F0020004F00700065006E0048006F00750073006500200074006F0020007500700064006100740065002000770065006200200070006F007200740061006C007300200069006E0020007200650061006C002000740069006D0065000000000058001B00010043006F006D00700061006E0079004E0061006D006500000000004B0069006E006C006500690067006800200046006F006C006B0061007200640020002600200048006100790077006100720064000000000064001E000100460069006C0065004400650073006300720069007000740069006F006E00000000004300230020005700650062002000460075006E006300740069006F006E007300200043004C005200200041007300730065006D0062006C0079000000300008000100460069006C006500560065007200730069006F006E000000000031002E0031002E0030002E003000000054001A00010049006E007400650072006E0061006C004E0061006D0065000000530051004C0050006F007200740061006C00460075006E006300740069006F006E0073004B00460048002E0064006C006C0000004800120001004C006500670061006C0043006F007000790072006900670068007400000043006F0070007900720069006700680074002000A90020002000320030003100370000005C001A0001004F0072006900670069006E0061006C00460069006C0065006E0061006D0065000000530051004C0050006F007200740061006C00460075006E006300740069006F006E0073004B00460048002E0064006C006C0000004C0016000100500072006F0064007500630074004E0061006D00650000000000530051004C0050006F007200740061006C00460075006E006300740069006F006E0073004B00460048000000340008000100500072006F006400750063007400560065007200730069006F006E00000031002E0031002E0030002E003000000038000800010041007300730065006D0062006C0079002000560065007200730069006F006E00000031002E0031002E0030002E00300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000005000000C000000B03D00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000;


GO
PRINT N'Creating [dbo].[fnWebSendXMLDataTest]...';


GO
CREATE FUNCTION [dbo].[fnWebSendXMLDataTest]
(@PostURL NVARCHAR (MAX), @Method NVARCHAR (MAX), @ContentType NVARCHAR (MAX), @XMLData XML, @P12CertPath NVARCHAR (MAX), @P12CertPass NVARCHAR (MAX), @Timeout INT, @SchemaURI NVARCHAR (MAX), @RootName NVARCHAR (MAX), @SuccessNode NVARCHAR (MAX), @SuccessValue NVARCHAR (MAX), @UniqueNodeValues BIT, @XPathReturnValue NVARCHAR (MAX))
RETURNS 
     TABLE (
        [XMLPosted]      NVARCHAR (MAX) NULL,
        [XMLResponse]    XML            NULL,
        [Success]        BIT            NULL,
        [Failure]        BIT            NULL,
        [AdditionalInfo] NVARCHAR (MAX) NULL,
        [Started]        DATETIME       NULL,
        [Finished]       DATETIME       NULL,
        [Code]           NVARCHAR (100) NULL,
        [ReturnValue]    NVARCHAR (MAX) NULL)
AS
 EXTERNAL NAME [SQLPortalFunctionsKFH].[FunctionsKFH].[fnWebSendXMLDataTest]


GO
PRINT N'Update complete.';


GO
