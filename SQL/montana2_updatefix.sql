/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 100408
 Source Host           : localhost:3306
 Source Schema         : montana2

 Target Server Type    : MySQL
 Target Server Version : 100408
 File Encoding         : 65001

 Date: 18/12/2019 19:49:00
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for admincp_logs
-- ----------------------------
DROP TABLE IF EXISTS `admincp_logs`;
CREATE TABLE `admincp_logs`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `adminid` int(11) NOT NULL,
  `log` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `date` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 6 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of admincp_logs
-- ----------------------------
INSERT INTO `admincp_logs` VALUES (1, 1, 'LegedPro sent this notice to the server:  sd:AD [Server]', '06:46:03 - 18.12.19', 1576640763);
INSERT INTO `admincp_logs` VALUES (2, 1, 'LegedPro sent this notice to the server:  /fucked [Server]', '06:46:16 - 18.12.19', 1576640776);
INSERT INTO `admincp_logs` VALUES (3, 1, 'LegedPro sent this notice to the server:  lark fff [Server]', '06:46:25 - 18.12.19', 1576640785);
INSERT INTO `admincp_logs` VALUES (4, 1, 'LegedPro sent this notice to the server:  :DSA:DAS:DSA [Server]', '06:46:31 - 18.12.19', 1576640791);
INSERT INTO `admincp_logs` VALUES (5, 1, 'LegedPro sent this notice to the server:  fuck off :D [Server]', '06:51:08 - 18.12.19', 1576641068);

-- ----------------------------
-- Table structure for anticheat_logs
-- ----------------------------
DROP TABLE IF EXISTS `anticheat_logs`;
CREATE TABLE `anticheat_logs`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT 0,
  `description` varchar(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `timestamp` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for carepackage
-- ----------------------------
DROP TABLE IF EXISTS `carepackage`;
CREATE TABLE `carepackage`  (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `itemcode` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `price` int(10) NOT NULL,
  `method` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `itemdays` int(10) NOT NULL,
  `loseitem1` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `loseitemdays1` int(10) NOT NULL,
  `loseitem2` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `loseitemdays2` int(10) NOT NULL,
  `loseitem3` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `loseitemdays3` int(10) NOT NULL,
  `loseitem4` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `loseitemdays4` int(10) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for changenick_logs
-- ----------------------------
DROP TABLE IF EXISTS `changenick_logs`;
CREATE TABLE `changenick_logs`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL,
  `oldnick` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `newnick` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `date` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `timestamp` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of changenick_logs
-- ----------------------------
INSERT INTO `changenick_logs` VALUES (1, 1, 'LegendWaRR', 'LegedPro', '06:42:47 - 18.12.19', 1576640567);

-- ----------------------------
-- Table structure for clans
-- ----------------------------
DROP TABLE IF EXISTS `clans`;
CREATE TABLE `clans`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'Montana',
  `maxusers` int(11) NOT NULL DEFAULT 20,
  `count` int(11) NOT NULL DEFAULT 0,
  `win` int(11) NOT NULL DEFAULT 0,
  `lose` int(11) NOT NULL DEFAULT 0,
  `exp` int(11) NOT NULL DEFAULT 0,
  `announcment` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `description` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `iconid` int(11) NOT NULL,
  `creationtime` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of clans
-- ----------------------------
INSERT INTO `clans` VALUES (1, 'Clanwar', 20, 1, 0, 0, 0, 'Troopers, lets go to the attack!', 'Welcome on Clan System!', 21015010, 1576640592);

-- ----------------------------
-- Table structure for clans_clanwars
-- ----------------------------
DROP TABLE IF EXISTS `clans_clanwars`;
CREATE TABLE `clans_clanwars`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `clanid1` int(11) NOT NULL DEFAULT 0,
  `clanid2` int(11) NOT NULL DEFAULT 0,
  `score` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `clanwon` int(11) NOT NULL,
  `date` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `timestamp` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for clans_invite
-- ----------------------------
DROP TABLE IF EXISTS `clans_invite`;
CREATE TABLE `clans_invite`  (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `userid` int(10) NOT NULL DEFAULT 0,
  `clanid` int(10) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for equipment
-- ----------------------------
DROP TABLE IF EXISTS `equipment`;
CREATE TABLE `equipment`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ownerid` int(11) NOT NULL,
  `class0` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'DA02,DB01,DF01,DR01,^,^,^,^',
  `class1` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'DA02,DB01,DF01,DQ01,^,^,^,^',
  `class2` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'DA02,DB01,DG05,DN01,^,^,^,^',
  `class3` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'DA02,DB01,DC02,DN01,^,^,^,^',
  `class4` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'DA02,DB01,DJ01,DL01,^,^,^,^',
  `inventory` varchar(5000) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  `storage` varchar(5000) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of equipment
-- ----------------------------
INSERT INTO `equipment` VALUES (1, 1, 'DA02,DB01,DF01,DR01,^,^,^,^', '^,DB01,I000,DQ01,I006,^,^,^', 'DA02,DB01,DG05,DN01,^,^,^,^', '^,DB01,I004,DN01,I001,^,^,^', 'DA02,DB01,DJ01,DL01,^,^,^,^', 'DF69-1-1-19122502-0,DE55-1-1-19122502-0,DC48-1-1-19122502-0,DE09-1-1-19122502-0,DC84-1-1-19122502-0,CA01-1-1-20022302-0,DF56-1-1-19122502-0,DJ13-1-1-19122502-0,DF35-1-1-20010202-0,DC33-1-1-20010202-0,DG08-1-1-20010202-0,DJ33-1-1-20010202-0,DN03-1-1-20010202-0,DF87-1-1-20011702-0,DG80-1-1-20011702-0,^,DJ39-1-1-33092502-0,CI01-1-1-20020102-0,CJ01-1-1-20021602-0,CJ02-1-1-20011702-0,DZ01-1-1-20021602-0,DB17-1-1-20011702-0,DA08-1-1-20011702-0,CD07-1-1-20011702-0,CC72-1-1-19122502-0,CC02-1-1-20021602-0,CF01-1-1-20011702-0,DS01-1-1-19122502-0,DB33-1-1-20011702-0,CD01-1-1-20011702-0,CD02-1-1-20011702-0,CB02-1-1-29102602-1,DF84-1-1-19122503-0,^,CC76-1-1-29102602-0,DT34-1-1-20010202-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^', '^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^');

-- ----------------------------
-- Table structure for friends
-- ----------------------------
DROP TABLE IF EXISTS `friends`;
CREATE TABLE `friends`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id1` int(11) NOT NULL DEFAULT 0,
  `id2` int(11) NOT NULL DEFAULT 0,
  `requesterid` int(11) NOT NULL DEFAULT 0,
  `status` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for gunsmith
-- ----------------------------
DROP TABLE IF EXISTS `gunsmith`;
CREATE TABLE `gunsmith`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gameid` int(11) NOT NULL DEFAULT 0,
  `item` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `rare` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `required_items` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `lose_items` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `required_materials` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `cost` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 7 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Records of gunsmith
-- ----------------------------
INSERT INTO `gunsmith` VALUES (1, 0, 'DE15', 'DE16', 'DB04,DC09,DC01', 'DC16', '12,9,6', 12000);
INSERT INTO `gunsmith` VALUES (2, 1, 'DF72', 'DF73', 'DB03,DF02,DF06', 'DC17', '9,12,6', 10000);
INSERT INTO `gunsmith` VALUES (3, 2, 'DG67', 'DG68', 'DB02,DG03,DG27', 'DC09', '6,9,12', 15000);
INSERT INTO `gunsmith` VALUES (4, 3, 'DD06', 'DD07', 'DB03,DD02,DD01', 'DF05', '12,9,9', 13000);
INSERT INTO `gunsmith` VALUES (5, 4, 'DJ18', 'DJ19', 'DB14,DJ03,DK01', 'DJ12', '12,15,18', 18000);
INSERT INTO `gunsmith` VALUES (6, 5, 'DB28', 'DB29', 'DB14,DB02,DB04', 'DB09', '6,6,6', 10000);

-- ----------------------------
-- Table structure for hwid_bans
-- ----------------------------
DROP TABLE IF EXISTS `hwid_bans`;
CREATE TABLE `hwid_bans`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hwid` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for inbox
-- ----------------------------
DROP TABLE IF EXISTS `inbox`;
CREATE TABLE `inbox`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ownerid` int(11) NOT NULL,
  `itemcode` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `days` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for ingame_coupons
-- ----------------------------
DROP TABLE IF EXISTS `ingame_coupons`;
CREATE TABLE `ingame_coupons`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `dinars` bigint(20) NOT NULL DEFAULT 0,
  `cashs` bigint(20) NOT NULL DEFAULT 0,
  `items` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL COMMENT 'Structure: item,days-item,days',
  `used` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `userId` int(20) NOT NULL DEFAULT -1,
  `time` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for levelups
-- ----------------------------
DROP TABLE IF EXISTS `levelups`;
CREATE TABLE `levelups`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NULL DEFAULT NULL,
  `oldlevel` int(11) NULL DEFAULT NULL,
  `newlevel` int(11) NULL DEFAULT NULL,
  `premium` int(11) NULL DEFAULT NULL,
  `timestamp` bigint(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for macs_ban
-- ----------------------------
DROP TABLE IF EXISTS `macs_ban`;
CREATE TABLE `macs_ban`  (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `mac` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for maps
-- ----------------------------
DROP TABLE IF EXISTS `maps`;
CREATE TABLE `maps`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `mapid` int(11) NOT NULL,
  `name` varchar(80) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `flags` int(4) NOT NULL,
  `defaultflags` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `vehicles` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 99 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of maps
-- ----------------------------
INSERT INTO `maps` VALUES (1, 52, '28th St', 2, '1|0', 'EA03;EN01;EN01;EN01;EN01;EN16');
INSERT INTO `maps` VALUES (2, 20, 'Alberon', 2, '0|1', 'EA01;EA01');
INSERT INTO `maps` VALUES (3, 95, 'ArmyHospital', 2, '0|1', 'EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EA03;EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (4, 37, 'Artifact', 2, '1|0', '');
INSERT INTO `maps` VALUES (5, 25, 'Bandar', 7, '0|1', 'ED01;EL01;EM01;EC01;EA01;EL01;EM01;EA01;EG02;EG02;EA01;EA01;EL01;EL01;EL01;EL01;EA01;EA01;EA01;ED01;EC01;EC01;EC01;EC01;EC01;ED05;EF04;EF04;EM01;EL01;EC01');
INSERT INTO `maps` VALUES (6, 64, 'BanishGarden', 2, '0|1', '');
INSERT INTO `maps` VALUES (7, 81, 'BanishGarden2', 2, '0|1', '');
INSERT INTO `maps` VALUES (8, 51, 'Barrio', 2, '0|1', 'EA03;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (9, 61, 'Bazaar', 2, '1|0', '');
INSERT INTO `maps` VALUES (10, 33, 'Beringia', 2, '0|1', '');
INSERT INTO `maps` VALUES (11, 58, 'BioLab', 2, '0|1', '');
INSERT INTO `maps` VALUES (12, 46, 'BlindBullet', 2, '0|1', 'EA03;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (14, 22, 'Bloc', 2, '1|0', '');
INSERT INTO `maps` VALUES (15, 32, 'BlueStorm', 2, '0|1', '');
INSERT INTO `maps` VALUES (16, 47, 'BrokenSunset', 2, '1|0', 'EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (18, 68, 'ByWay', 2, '1|0', '');
INSERT INTO `maps` VALUES (19, 13, 'Cadoro', 2, '0|1', '');
INSERT INTO `maps` VALUES (20, 16, 'Cantumira', 7, '0|1', 'EE04;ED04;EJ05;EC01;EJ05;EJ06;EE03;ED01;ED05;EB01;EB01;ED05;EL01;EM01');
INSERT INTO `maps` VALUES (21, 41, 'CentralSquare', 6, '0|1', 'ED05;EM01;EA01;EA01;EL01;EL01;ED06;EA01;EC01;EC01;EC01;EC01;EC01;EC01;EN01;EN01');
INSERT INTO `maps` VALUES (22, 30, 'Cloud Forest', 4, '1|0', 'ED08;ED08;EA01;ED01;EA01;EC01;EC01;ED01');
INSERT INTO `maps` VALUES (23, 28, 'Cold Cave', 2, '0|1', '');
INSERT INTO `maps` VALUES (24, 11, 'Conturas', 10, '1|0', 'EK02;EK04;EK02;EJ05;EK02;EK02;EK04;EK02;ED04;EB01;EK02;EK02;EK02;EC01;ED04;EB01;EK02;EK02;EK04;EK04;EK02;EK02;EJ05;EC01;ED04;ED04;ED04;ED04;EK02;EK02;EK02;EK04;EK02;EK02;EK02;EK04;EB01;EB01');
INSERT INTO `maps` VALUES (25, 83, 'Countryside', 2, '0|1', 'EE18;EG06;EH09;EE20;EE20;EE18;EH09;EE20;EE20;EE20;EE20;EE20;EE20;EE19;EE19;EE19;EE19;EE19;EE19;EH09;EH09;EH09;EH09;EH09;EH08;EH08;EH08;EH08;EH08;EH08;EH08;EE18;EE18;EE18;EE18;EE18;EE17;EE17;EE17;EE17;EE17;EE17;EE17;EG06;EG06;EG06;EG06;EG06;EG06;EG04;EG04;');
INSERT INTO `maps` VALUES (26, 31, 'Crater', 8, '5|0', 'EJ06;EB01;EB04;EE03;ED05;ED06;EA01;EB01;EC01;EC01;ED06;EC01;EB04;EB01;EA01;EA01;EK02;EI02;EC01;EC01;EJ04;ED08;EC01;ED06;EE04;EE02;EA01;EC01;EF04;EC01;EI02;EJ07;EB04;EK04;EJ06;EE04;EC01');
INSERT INTO `maps` VALUES (27, 71, 'Crater_Dogfight', 2, '1|0', 'EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EB04;EB04;EB04;EB04;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EJ03;EB04;EJ03;EB04');
INSERT INTO `maps` VALUES (28, 35, 'Darkglow', 2, '0|1', '');
INSERT INTO `maps` VALUES (29, 45, 'DayLight', 4, '1|0', 'EG02');
INSERT INTO `maps` VALUES (30, 48, 'DayOne', 2, '0|1', 'EA03;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (31, 66, 'DeathHill', 9, '0|1', 'ED08;ED08;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;ED01;EC01;EC01');
INSERT INTO `maps` VALUES (32, 39, 'Decay', 2, '0|1', '');
INSERT INTO `maps` VALUES (33, 67, 'Denkmal', 2, '1|0', '');
INSERT INTO `maps` VALUES (34, 36, 'Disturm', 9, '8|0', 'EK02;ED05;ED08;EM01;EL01;EM01;EM01;EE03;ED05;EB01;EL01;EB04;EC01;EB01;EB04;EE03;EC01;EA01;EC01;EL03;EE04;EK02;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EA01;EA01;EC01;EC01;EN01;EB04;EB01');
INSERT INTO `maps` VALUES (35, 44, 'Dotonbori', 2, '1|0', '');
INSERT INTO `maps` VALUES (36, 2, 'Emblem', 5, '0|1', 'EC01;EC01;EF04;EC01;EC01;EF04;ED01;ED01;ED01;EA01;EA01;EA01;EA01');
INSERT INTO `maps` VALUES (37, 7, 'Engrene', 12, '0|5', 'EK01;EJ07;EK01;EB01;EB01;EI01;EI01;EB01;EJ08;EB01;EJ08;EI01;EK01;EK01;EJ07;EB01;EI01;EB01;EI02;EI02;EC01;EC01;EA01;EA01;ED02;EE04;EE04;ED02;EE04;EE04;EC01;EC01;EB01;EB01;EB04;EB04');
INSERT INTO `maps` VALUES (38, 75, 'FAVELA', 2, '0|1', '');
INSERT INTO `maps` VALUES (40, 96, 'GuardPost', 2, '0|1', 'EA02;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (41, 82, 'Hanok_village', 2, '1|0', '');
INSERT INTO `maps` VALUES (42, 10, 'Harbor Elia', 5, '4|1', '');
INSERT INTO `maps` VALUES (43, 1, 'Harbor Ida', 4, '1|0', '');
INSERT INTO `maps` VALUES (44, 3, 'Havana', 6, '3|2', 'EE04;EE04;EG02;EG02;EA01;EA01;ED01;ED01');
INSERT INTO `maps` VALUES (45, 56, 'Institute_R', 8, '0|1', 'EN17;EN18;EN19;EN55;EN19;EN54;EN53;EN51;EN51;EN51;EN56;EN57');
INSERT INTO `maps` VALUES (46, 77, 'Highlands', 10, '0|1', 'ED02;EE04;ED14;EE06;EE04;ED14;EC01;EC01;EH07;ED02;ED14;EE03;EE03;ED14;EH07;EE06;EJ04;EJ04;ED14;ED14;EC01;EC01;EC01;EC01;EJ03;EC01;EC01;ED08;ED08;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01');
INSERT INTO `maps` VALUES (47, 62, 'Jiufen', 2, '0|1', '');
INSERT INTO `maps` VALUES (48, 50, 'Junkyard', 2, '0|1', 'EA03;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (49, 57, 'KalyeDos', 2, '1|0', '');
INSERT INTO `maps` VALUES (50, 19, 'Karaqum', 2, '1|0', '');
INSERT INTO `maps` VALUES (51, 63, 'Kashgar', 7, '0|1', 'EG01;EE08;EL01;EL01;ED08;EG02;EC01;EG01;EE08;EE04;EE04;EC01;EG02;ED09;EC01;ED08;ED09;EJ06;EB01;EB01;EB04;EB04;ED02;EC01;EC01;EC01;EC01;EC01');
INSERT INTO `maps` VALUES (52, 23, 'Khali', 2, '1|0', '');
INSERT INTO `maps` VALUES (53, 59, 'Kharif', 2, '1|0', '');
INSERT INTO `maps` VALUES (54, 17, 'Khyber', 2, '0|1', '');
INSERT INTO `maps` VALUES (55, 53, 'Lighthouse', 2, '1|0', 'EA03;EN16');
INSERT INTO `maps` VALUES (56, 43, 'LostTemple', 2, '1|0', '');
INSERT INTO `maps` VALUES (57, 80, 'Malta', 2, '1|0', '');
INSERT INTO `maps` VALUES (58, 12, 'Marien', 2, '1|0', '');
INSERT INTO `maps` VALUES (59, 84, 'Mist', 2, '0|1', '');
INSERT INTO `maps` VALUES (60, 85, 'ModernHouse', 2, '0|1', '');
INSERT INTO `maps` VALUES (61, 0, 'Montana', 3, '0|2', 'ED01;ED01;ED01;EA01;EA01;ED01');
INSERT INTO `maps` VALUES (62, 76, 'MUSTANG', 3, '0|1', '');
INSERT INTO `maps` VALUES (63, 21, 'Nerbil', 2, '0|1', '');
INSERT INTO `maps` VALUES (64, 69, 'Odyssey', 2, '1|0', 'EC01;EC01;ED01;ED01;EM01;EM01;ED01;ED01;EC01;EC01;EM01;EM01;EC01;EC01;EC01;EC01');
INSERT INTO `maps` VALUES (65, 4, 'Ohara', 5, '0|1', 'ED01;ED01;EA01;EA01;EA01;EA01;EA01;EA01;EE04;EE04;EE04;EE04;ED01;EE04;EC01;EC01;EC01;EC01;EA01;EA01;ED08;ED08');
INSERT INTO `maps` VALUES (66, 49, 'OutPost', 2, '0|1', 'EN01;EN01;EN01;EN01;EN01;EN01;EN01');
INSERT INTO `maps` VALUES (67, 8, 'Pargona', 10, '0|1', 'EJ06;EI01;EB01;EB01;ED01;EB01;EB01;EB01;EB01;EI01;EM01;EJ05;EL01;EJ05;EJ05;EB01;EJ06;EL01;EM01;ED01;EB01;EC01;EC01;EL01;EL01;EA01;EA01;EA01;EA01');
INSERT INTO `maps` VALUES (68, 72, 'Pargona_DOGFIGHT', 2, '1|0', 'EB01;EB01;EB01;EJ04;EJ05;EJ05;EB01;EJ03;EJ03;EJ06;EJ07;EJ05;EJ07;EJ04;EJ04;EJ04;EJ03;EJ04;EJ04;EJ03;EJ06;EJ05;EJ07;EJ04;EJ04;EJ07;EJ05;EJ05');
INSERT INTO `maps` VALUES (69, 9, 'Pargona East', 10, '9|0', 'EL01;EJ06;EJ05;EJ05;EB01;EB01;EL01;EL01;EM01;EI01;EJ05;EJ05;EC01;EB01;EJ06;EJ05;EJ05;EL01;EL01;EM01;EL01;ED02;ED01;ED02;EB01;ED02;ED01;EI01;EC01;ED02');
INSERT INTO `maps` VALUES (70, 34, 'Paroho', 4, '0|2', 'EG02;EC01;EC01;EG02;EF04');
INSERT INTO `maps` VALUES (71, 55, 'POW Camp', 8, '0|1', 'EN01;EN01;EN01;EN01;EN01;EN17;EN18;EN21;EN20;EN19;EN19;EN19;EN35;EN35');
INSERT INTO `maps` VALUES (72, 5, 'Ravello', 4, '2|0', '');
INSERT INTO `maps` VALUES (73, 6, 'Ravello 2nd St.', 4, '2|0', '');
INSERT INTO `maps` VALUES (74, 27, 'Red Clover', 2, '1|0', '');
INSERT INTO `maps` VALUES (75, 65, 'Redsign', 6, '0|1', 'EN31;EN22;EN23;EN24;EN25;EN27;EN28;EN24;EN25;EN26;EN30;EN30;EN26;EN33;EN28;EN29;EN29;EN32;EN27;EN01;EN28;EN28;EN28;EN28;EN36;EN37;EN38;EN39;EN40;EN42;EN41;EN43');
INSERT INTO `maps` VALUES (76, 29, 'Rusty Nails', 2, '0|1', '');
INSERT INTO `maps` VALUES (77, 54, 'Second_Lab', 2, '0|1', 'EA03;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN01;EN16');
INSERT INTO `maps` VALUES (78, 42, 'SiegeWar', 4, '0|2', 'EE04;ED06;ED08;EA01;EA02;EA02;ED01;EN03;EN02;EN02;EN04;ED02;EE02;ED08;EA02;EA02;EA02;EA02;EA02;EA02;ED08;EA02;EE02;EN05;EN05;EN05;EC01;EN06');
INSERT INTO `maps` VALUES (79, 60, 'SiegeWar2', 4, '0|1', 'EN10;EN11;EN08;EN14;EN07;EN08');
INSERT INTO `maps` VALUES (80, 40, 'SkillPointer', 3, '1|0', 'EE07;EE01;EE01;EH01;EH01;EE01;EE01;EE07;EE07;EH01;EH01;EE07');
INSERT INTO `maps` VALUES (81, 99, 'Stadium', 4, '0|1', 'EC03');
INSERT INTO `maps` VALUES (82, 74, 'STATION', 2, '1|0', '');
INSERT INTO `maps` VALUES (83, 24, 'Thamugadi', 10, '0|1', 'EB01;EI02;EI01;EE04;EJ08;EC01;EB01;EB01;EJ03;EC01;EE04;EI02;EE03;EE02;EJ03;EB01;EB04;EB04;EB04;EJ03;EI01;EB04');
INSERT INTO `maps` VALUES (84, 78, 'UrbanPark', 10, '0|1', 'ED13;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EF06;EF06;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01;EC01');
INSERT INTO `maps` VALUES (85, 14, 'Velruf', 2, '1|0', '');
INSERT INTO `maps` VALUES (86, 79, 'Venezia', 2, '1|0', '');
INSERT INTO `maps` VALUES (87, 26, 'Vitious', 2, '0|1', '');
INSERT INTO `maps` VALUES (88, 90, 'WaterLand', 2, '1|0', 'EN65');
INSERT INTO `maps` VALUES (89, 91, 'WaterLand2', 2, '1|0', 'EN65');
INSERT INTO `maps` VALUES (90, 38, 'Winter Forest', 4, '3|0', 'EA01;ED08;ED08;ED01;EC01;EC01;EA01;EC01;EC01;ED01');
INSERT INTO `maps` VALUES (91, 440, 'W_Marien', 2, '1|0', '');
INSERT INTO `maps` VALUES (92, 15, 'Xauen', 2, '1|0', '');
INSERT INTO `maps` VALUES (93, 710, 'XJiufen', 2, '0|1', 'EN59;EN59;EN59;EN59;EN59;EN59;EN01');
INSERT INTO `maps` VALUES (94, 7100, 'XMarien', 2, '1|0', '');
INSERT INTO `maps` VALUES (95, 610, 'XVelruf', 2, '1|0', '');
INSERT INTO `maps` VALUES (96, 70, 'Zadar', 5, '1|0', 'EC01;EC01;EJ06;EJ06;EC01;EC01;ED01;ED01;EE06;EE06;EE06;ED01;ED02;ED02;ED01;EB01;EB01');
INSERT INTO `maps` VALUES (97, 18, 'Zakhar', 5, '0|1', 'ED04;EE04;ED05;ED04;ED01;ED05;EJ03;EJ06;ED01;EE03;EA01;EB01;EB01;EA01;EA01');
INSERT INTO `maps` VALUES (98, 73, 'ZEROPOINT', 8, '0|1', 'EE16;ED12;ED11;EJ13;EB05;ED11;ED12;EE16;EB05;ED12;ED12;ED12;EJ11;EJ13;EJ11;ED12;EE14;EE14;EF05;EC02;EC02;EC02;EC02;EC02;EC02;EI05;EC02;EC02;EC02;EC02');

-- ----------------------------
-- Table structure for notices
-- ----------------------------
DROP TABLE IF EXISTS `notices`;
CREATE TABLE `notices`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `message` varchar(320) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `deleted` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 8 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of notices
-- ----------------------------
INSERT INTO `notices` VALUES (1, 'WrMontana is back!', '0');
INSERT INTO `notices` VALUES (2, 'Invite your friends...More people more fun!', '0');
INSERT INTO `notices` VALUES (3, 'Dont do the camper! Play it :D', '0');
INSERT INTO `notices` VALUES (4, 'We want to see many videos on YouTube :P!', '0');
INSERT INTO `notices` VALUES (5, 'You have found a bug? Report it!', '0');
INSERT INTO `notices` VALUES (6, 'Do not ask for Premium/Weapons/Coin/Cash', '0');
INSERT INTO `notices` VALUES (7, 'Retails & Special weapons on the webshop!', '0');

-- ----------------------------
-- Table structure for outbox
-- ----------------------------
DROP TABLE IF EXISTS `outbox`;
CREATE TABLE `outbox`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ownerid` int(11) NOT NULL,
  `itemcode` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `days` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `count` int(11) NOT NULL DEFAULT 1,
  `timestamp` bigint(20) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 11 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for promocodes
-- ----------------------------
DROP TABLE IF EXISTS `promocodes`;
CREATE TABLE `promocodes`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` bigint(20) NULL DEFAULT NULL,
  `itemcode` char(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `promocode` longtext CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `name` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `days` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `used` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `bindedid` bigint(20) NOT NULL DEFAULT -1,
  `fromid` bigint(20) NOT NULL DEFAULT -1,
  `daily` enum('N','Y') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'N',
  `timestamp` bigint(20) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for purchases_logs
-- ----------------------------
DROP TABLE IF EXISTS `purchases_logs`;
CREATE TABLE `purchases_logs`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT 0,
  `log` char(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `timestamp` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for reset_keys
-- ----------------------------
DROP TABLE IF EXISTS `reset_keys`;
CREATE TABLE `reset_keys`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT 0,
  `reset_key` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `used` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `timestamp` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for serverinfo
-- ----------------------------
DROP TABLE IF EXISTS `serverinfo`;
CREATE TABLE `serverinfo`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `value` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 6 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Records of serverinfo
-- ----------------------------
INSERT INTO `serverinfo` VALUES (1, 'uptime', '0d, 0h, 0m');
INSERT INTO `serverinfo` VALUES (2, 'lastupdate', '1576642931');
INSERT INTO `serverinfo` VALUES (3, 'expexpire', '19:00 25/09/16');
INSERT INTO `serverinfo` VALUES (4, 'expevent', '0');
INSERT INTO `serverinfo` VALUES (5, 'exprate', '200%, 400%');

-- ----------------------------
-- Table structure for servers
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `serverid` int(11) NOT NULL,
  `name` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `ip` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `slot` int(5) NOT NULL DEFAULT 10,
  `clanwar` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `flag` enum('0','1','2','3','4','5') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `minrank` enum('1','2','3','4','5','6') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `visible` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 3 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Records of servers
-- ----------------------------
INSERT INTO `servers` VALUES (1, 1, 'WrMontana', '127.0.0.1', 250, '0', '1', '1', '1');
INSERT INTO `servers` VALUES (2, -1, 'WebServer', '127.0.0.1', 0, '0', '0', '1', '0');

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `password` varchar(32) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `salt` varchar(24) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `nickname` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `email` varchar(520) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `rank` enum('0','1','2','3','4','5','6') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '1',
  `exp` int(20) NOT NULL,
  `dinar` bigint(20) NOT NULL,
  `cash` bigint(20) NOT NULL,
  `coin` int(11) NOT NULL DEFAULT 0,
  `kills` int(11) NOT NULL DEFAULT 0,
  `deaths` int(11) NOT NULL DEFAULT 0,
  `wonMatchs` int(11) NOT NULL DEFAULT 0,
  `lostMatchs` int(11) NOT NULL DEFAULT 0,
  `headshots` int(11) NOT NULL DEFAULT 0,
  `ticketid` int(11) NOT NULL DEFAULT 0,
  `killcount` int(11) NOT NULL DEFAULT 0,
  `serverid` int(11) NOT NULL DEFAULT -1,
  `loginEventToday` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `loginEventProgress` enum('0','1','2','3','4','5','6','7') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `premium` enum('0','1','2','3','4') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `online` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `banned` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `eventcount` int(11) NOT NULL DEFAULT 0,
  `donationexpire` int(20) NOT NULL DEFAULT -1,
  `active` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `premiumExpire` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '-1',
  `muted` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `mutedExpire` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '-1',
  `chat_color` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '',
  `coupons` int(11) NOT NULL DEFAULT 0,
  `todaycoupon` int(11) NOT NULL DEFAULT 0,
  `coupontime` int(11) NOT NULL DEFAULT 0,
  `randombox` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `lastmac` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `retailcode` char(4) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'NULL',
  `retailclass` enum('0','1','2','3','4') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `lasthwid` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `clanid` int(11) NOT NULL DEFAULT -1,
  `clanrank` int(11) NOT NULL DEFAULT 0,
  `clanjoindate` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'Unknown',
  `country` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '',
  `firstlogin` int(10) NOT NULL DEFAULT 0,
  `broadtoday` int(10) NOT NULL DEFAULT 0,
  `broadday` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `websession` int(11) NOT NULL DEFAULT -1,
  `acpsession` int(11) NOT NULL DEFAULT -1,
  `storageInventory` int(11) NOT NULL DEFAULT 0,
  `promocodes` int(11) NOT NULL DEFAULT 0,
  `promoday` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `lastipaddress` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `bantime` int(10) NOT NULL DEFAULT -1,
  `banreason` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'Unknown',
  `lastjoin` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'Never',
  `jointimestamp` bigint(20) NOT NULL,
  `lastdaystats` char(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `medalid` varchar(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '-1',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users
-- ----------------------------
INSERT INTO `users` VALUES (1, 'Neon', '281ab141a12f67f5238719cd876ce96e', '123456', 'LegedPro', '', '6', 10005290, 1107191, 9930899, 999999, 0, 0, 0, 0, 0, -1, 0, 1, '0', '0', '3', '0', '0', 0, -1, '0', '1580528344', '0', '-1', '', 1, 1, 30, '0', '022a717a0503', 'NULL', '0', '', 1, 2, '18.12.2019', '', 1, 0, '0', -1, -1, 0, 0, '0', 'U mad bro?', -1, 'Unknown', '1576642867', 0, '18-12-2019', '-1');

-- ----------------------------
-- Table structure for users_costumes
-- ----------------------------
DROP TABLE IF EXISTS `users_costumes`;
CREATE TABLE `users_costumes`  (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `ownerid` int(10) NOT NULL,
  `class_0` char(150) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'BA01,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  `class_1` char(150) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'BA02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  `class_2` char(150) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'BA03,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  `class_3` char(150) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'BA04,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  `class_4` char(150) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT 'BA05,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  `inventory` varchar(5000) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of users_costumes
-- ----------------------------
INSERT INTO `users_costumes` VALUES (1, 1, 'BA11,^,^,BD02,^,BF44,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^', 'BA12,^,^,BD02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^', 'BA13,^,^,BD02,^,BF44,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^', 'BA1A,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^', 'BA15,^,^,BD02,^,BF44,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^', 'BD02-1-1-20010102-0-0-0-0-0,BF44-1-1-20010102-0-0-0-0-0,BA11-1-1-19122502-0-0-0-0-0,BA12-1-1-19122502-0-0-0-0-0,BA13-1-1-19122502-0-0-0-0-0,BA14-1-1-19122502-0-0-0-0-0,BA15-1-1-19122502-0-0-0-0-0,BA1A-1-1-20021602-0-0-0-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^');

-- ----------------------------
-- Table structure for users_events
-- ----------------------------
DROP TABLE IF EXISTS `users_events`;
CREATE TABLE `users_events`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` bigint(20) NOT NULL,
  `eventid` int(11) NOT NULL,
  `timestamp` bigint(20) NOT NULL,
  `permanent` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT '0' COMMENT 'Should it be never deleted?',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for users_matchs
-- ----------------------------
DROP TABLE IF EXISTS `users_matchs`;
CREATE TABLE `users_matchs`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT 0,
  `Nickname` char(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `exp` bigint(20) NOT NULL DEFAULT 0,
  `totalexp` bigint(20) NOT NULL,
  `dinar` bigint(20) NOT NULL DEFAULT 0,
  `roomtype` int(10) NOT NULL DEFAULT 0,
  `channel` enum('0','1','2','3','4') CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
  `premium` enum('0','1','2','3','4') CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
  `mode` int(11) NOT NULL DEFAULT 0,
  `kills` int(11) NOT NULL DEFAULT 0,
  `deaths` int(11) NOT NULL DEFAULT 0,
  `mapid` int(11) NOT NULL DEFAULT 0,
  `level` int(11) NOT NULL DEFAULT -1,
  `date` char(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '-1',
  `headshots` int(11) NOT NULL,
  `day` datetime(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for users_stats
-- ----------------------------
DROP TABLE IF EXISTS `users_stats`;
CREATE TABLE `users_stats`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userid` int(11) NOT NULL DEFAULT 0,
  `nickname` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `totalexp` bigint(20) NOT NULL DEFAULT 0,
  `exp` bigint(20) NOT NULL DEFAULT 0,
  `dinar` bigint(20) NOT NULL DEFAULT 0,
  `kills` bigint(20) NOT NULL DEFAULT 0,
  `deaths` bigint(20) NOT NULL DEFAULT 0,
  `headshots` bigint(20) NOT NULL DEFAULT 0,
  `premium` bigint(20) NOT NULL DEFAULT 0,
  `country` char(20) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '--',
  `date` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `timestamp` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Records of users_stats
-- ----------------------------
INSERT INTO `users_stats` VALUES (1, 1, 'LegedPro', 10005290, 5291, 1692, 0, 0, 0, 3, '', '18-12-2019', 1576642685);

-- ----------------------------
-- Table structure for vehicles
-- ----------------------------
DROP TABLE IF EXISTS `vehicles`;
CREATE TABLE `vehicles`  (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `code` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `name` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `maxhealth` int(11) NOT NULL,
  `respawntime` int(11) NOT NULL,
  `seats` char(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0:0:NULL',
  `joinable` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 165 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of vehicles
-- ----------------------------
INSERT INTO `vehicles` VALUES (1, 'EA01', 'FIXED_CAL50', 3000, 20, '30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (2, 'EA02', 'FIXED_TOW', 3000, 20, '1,999:FJ06-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (3, 'EA03', 'FIXED_MINIGUN', 3000, 20, '30000,0:FB05-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (4, 'EA04', 'FIXED_SENTRYGUN', 3000, 20, '999,0:FB06-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (5, 'EB01', 'FIXED_OERLIKON', 10000, 20, '30000,0:FD01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (6, 'EB02', 'FIXED_HAWK_MISSILE', 3000, 20, '20,1:FI03-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (7, 'EB03', 'FIXED_PATRIOT_MISSILE', 1500, 20, '20,1:FI01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (8, 'EB04', 'FIXED_SG', 1500, 20, '4,10:FI05-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (9, 'EB05', 'FIXED_OERLIKON_WINTER', 10000, 20, '30000,0:FD01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (10, 'EC01', 'MOTORCYCLE_CR125', 2500, 20, '0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (11, 'EC02', 'MOTORCYCLE_CR125_WINTER', 2500, 20, '0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (12, 'EC03', 'MOTORCYCLE_SoccerBall', 1000000, 20, '0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (13, 'ED01', 'CAR_HUMVEE_CAL50', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (14, 'ED02', 'CAR_HUMVEE_TOW', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;16,1:FJ01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (15, 'ED03', 'CAR_HUMVEE_MK19', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;50,0:FH01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (16, 'ED04', 'CAR_HUMVEE_AVENGER', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;30000,0:FB01-4,5:FI02', '1');
INSERT INTO `vehicles` VALUES (17, 'ED05', 'CAR_LUCHS', 4000, 20, '500,0:FC01-0,0:FA01;2000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (18, 'ED06', 'CAR_DPV', 3000, 20, '0,0:FA01-0,0:FA01;5,5:FH01-0,0:FA01;999,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (19, 'ED07', 'CAR_ATV', 2500, 20, '0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (20, 'ED08', 'CAR_60TRUCK_COVER', 7000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (21, 'ED09', 'CAR_HUMVEE_CUPOLA', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (22, 'ED10', 'CAR_SNOWMOBILE', 3000, 20, '2000,0:FB02-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (23, 'ED11', 'CAR_60TRUCK_COVER_WINTER', 7000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (24, 'ED12', 'CAR_HUMVEE_CAL50_WINTER', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (25, 'ED13', 'CAR_ATVC', 20000, 20, '0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (26, 'ED14', 'CAR_M1128MGS', 4000, 20, '30,0:FF01-999,0:FE01;999,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (27, 'EE01', 'TANK_M1A1', 15000, 20, '1,999:FF03-30000,0:FC03;30000,0:FB04-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (28, 'EE02', 'TANK_LEOPARD', 4600, 20, '30,0:FF01-500,0:FE01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (29, 'EE03', 'TANK_LECLERC', 4550, 20, '30,0:FF01-500,0:FE01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (30, 'EE04', 'TANK_K1A1', 4500, 20, '30,0:FF01-500,0:FE01;30000,0:FB01-0,0:FA01;2000,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (31, 'EE05', 'TANK_90II', 4700, 20, '30,0:FF01-500,0:FE01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (32, 'EE06', 'TANK_MERKAVA_MK4', 6000, 20, '0,0:FA01-0,0:FA01;30,0:FF01-30,0:FI07;999,0:FB01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (33, 'EE07', 'TANK_JAPAN90', 15000, 20, '1,999:FF04-500,0:FC03', '1');
INSERT INTO `vehicles` VALUES (34, 'EE08', 'TANK_M1A2', 4500, 20, '30,0:FF01-800,0:FB01;2000,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (35, 'EE09', 'TANK_CM11', 4500, 20, '30,0:FF01-500,0:FE01;30000,0:FB01-0,0:FA01;2000,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (36, 'EE10', 'TANK_M8AGS', 4000, 20, '30,0:FF01-500,0:FE01;999,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (37, 'EE11', 'TANK_M551_SHERIDAN', 4000, 20, '30,0:FF01-500,0:FE01;999,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (38, 'EE12', 'TANK_AAV7A1', 5000, 20, '8,5:FH01-999,0:FB01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (39, 'EE13', 'TANK_T80U', 4500, 20, '30,0:FF05-500,0:FE02;30000,0:FD02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (40, 'EE14', 'TANK_M551_SHERIDAN_WINTER', 4000, 20, '30,0:FF01-500,0:FE01;999,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (41, 'EE15', 'TANK_K1A1_WINTER', 4500, 20, '30,0:FF01-500,0:FE01;30000,0:FB01-0,0:FA01;2000,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (42, 'EE16', 'TANK_T80U_WINTER', 4500, 20, '30,0:FF05-500,0:FE02;30000,0:FD02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (43, 'EE17', 'TANK_CHALLENGER2DER', 6000, 20, '1,999:FF06-1,10:FH02', '1');
INSERT INTO `vehicles` VALUES (44, 'EE18', 'TANK_CHALLENGER2NIU', 6000, 20, '1,999:FF06-1,10:FH02', '1');
INSERT INTO `vehicles` VALUES (45, 'EE19', 'TANK_M88A2DER', 3000, 20, '9999,0:FC05-1,6:FH03', '1');
INSERT INTO `vehicles` VALUES (46, 'EE20', 'TANK_M88A2NIU', 3000, 20, '9999,0:FC05-1,6:FH03', '1');
INSERT INTO `vehicles` VALUES (47, 'EF01', 'TANK_AMX10RC', 3700, 20, '30,0:FF02-500,0:FE01;30000,0:FB01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (48, 'EF02', 'TANK_CENTAURO', 3800, 20, '30,0:FF02-500,0:FE01;30000,0:FB01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (49, 'EF03', 'TANK_WIESEL_TOW', 3800, 20, '16,1:FJ01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (50, 'EF04', 'TANK_WIESEL_GUN', 3800, 20, '500,0:FC01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (51, 'EF05', 'TANK_WIESEL_GUN_WINTER', 3800, 20, '500,0:FC01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (52, 'EF06', 'TANK_WIESEL_GUN_SWAT', 2500, 20, '500,0:FC01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (53, 'EG01', 'TANK_M2A2', 4000, 20, '500,0:FC01-16,1:FJ01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (54, 'EG02', 'TANK_K200', 5000, 20, '0,0:FA01-0,0:FA01;30000,0:FB01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (55, 'EG03', 'TANK_CHINESE_TYPE_90', 4000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (56, 'EG04', 'TANK_BMP3DER', 3000, 20, '1,999:FF07-9999,0:FC04', '1');
INSERT INTO `vehicles` VALUES (57, 'EG06', 'TANK_BMP3NIU', 3000, 20, '1,999:FF07-9999,0:FC04', '1');
INSERT INTO `vehicles` VALUES (58, 'EH01', 'TANK_M109A6', 20000, 20, '1,999:FG01-0,0:FA01;30000,0:FC03-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (59, 'EH02', 'TANK_PZH2000', 4000, 20, '50,0:FG01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (60, 'EH03', 'TANK_K9', 4000, 20, '50,0:FG01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (61, 'EH04', 'TANK_CHINESE89', 4000, 20, '50,0:FG01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (62, 'EH05', 'TANK_MLRS', 4000, 20, '0,0:FA01-0,0:FA01;40,1:FJ03-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (63, 'EH06', 'AT_CANNON_105', 4000, 20, '50,0:FG02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (64, 'EH07', 'TANK_M109A6_Paladin', 2500, 20, '1,50:FJ07-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (65, 'EH08', 'CAR_G6DER', 2000, 20, '1,999:FG03-1,999:FG04', '1');
INSERT INTO `vehicles` VALUES (66, 'EH09', 'CAR_G6NIU', 2000, 20, '1,999:FG03-1,999:FG04', '1');
INSERT INTO `vehicles` VALUES (67, 'EI01', 'TANK_GEPARD', 4500, 20, '2000,0:FD01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (68, 'EI02', 'TANK_CHUNMA', 4000, 20, '8,2:FI05-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (69, 'EI03', 'TANK_ADATS', 4000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (70, 'EI04', 'TANK_BIHO', 4000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (71, 'EI05', 'TANK_CHUNMA_WINTER', 4000, 20, '8,2:FI05-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (72, 'EJ01', 'HEL_BLACKHAWK_MINIGUN2', 3200, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;999,0:FB03-0,0:FA01;999,0:FB03-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (73, 'EJ02', 'HEL_UH1_ROCKET', 3000, 20, '20,1:FJ04-20,1:FJ04;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (74, 'EJ03', 'HEL_APACHE', 3800, 20, '10,1:FI04-8,1:FJ02;999,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (75, 'EJ04', 'HEL_HIND', 3800, 20, '10,2:FJ03-0,0:FA01;999,0:FB04-2,5:FJ04', '1');
INSERT INTO `vehicles` VALUES (76, 'EJ05', 'HEL_500MD', 2700, 20, '999,0:FB04-10,2:FJ03;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (77, 'EJ06', 'HEL_BLACKHAWK_MINIGUN', 3200, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;999,0:FB03-0,0:FA01;999,0:FB03-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (78, 'EJ07', 'HEL_BLACKHAWK_MISSILE', 3800, 20, '8,3:FJ02-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (79, 'EJ08', 'HEL_CH47_CHINOOK', 15000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;30000,0:FB01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (80, 'EJ09', 'HEL_KA50HOKUM', 4000, 20, '10,3:FI06-10,3:FJ03', '1');
INSERT INTO `vehicles` VALUES (81, 'EJ10', 'HEL_BLACKHAWK_MINIGUN_WINTER', 3200, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;999,0:FB03-0,0:FA01;999,0:FB03-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (82, 'EJ11', 'HEL_BLACKHAWK_MISSILE_WINTER', 5000, 20, '8,3:FJ02-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (83, 'EJ12', 'HEL_APACHE_WINTER', 3800, 20, '10,1:FI06-8,1:FJ02;999,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (84, 'EJ13', 'HEL_CH47_CHINOOK_WINTER', 15000, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;30000,0:FB01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (85, 'EK01', 'AIRPLANE_A10', 3500, 20, '999,0:FC02-8,2:FL01', '1');
INSERT INTO `vehicles` VALUES (86, 'EK02', 'AIRPLANE_RAFALE', 3600, 20, '999,0:FC03-16,1:FI05', '1');
INSERT INTO `vehicles` VALUES (87, 'EK03', 'AIRPLANE_HARIER', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (88, 'EK04', 'AIRPLANE_KF15', 3700, 20, '8,2:FI04-16,1:FL02', '1');
INSERT INTO `vehicles` VALUES (89, 'EK05', 'AIRPLANE_STEALTH', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (90, 'EK06', 'AIRPLANE_F22', 4000, 20, '999,0:FC03-18,2:FI05', '1');
INSERT INTO `vehicles` VALUES (91, 'EK07', 'AIRPLANE_F16', 3600, 20, '999,0:FC03-16,1:FI05', '1');
INSERT INTO `vehicles` VALUES (92, 'EK08', 'AIRPLANE_ChingKuo', 3700, 20, '8,2:FI04-16,1:FL02', '1');
INSERT INTO `vehicles` VALUES (93, 'EK09', 'AIRPLANE_SU47', 4000, 20, '999,0:FC03-18,2:FI05', '1');
INSERT INTO `vehicles` VALUES (94, 'EL01', 'BOAT_LSSC', 3500, 20, '0,0:FA01-0,0:FA01;2000,0:FB02-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (95, 'EL02', 'BOAT_AIRCRAFT_CARRIER', 3500, 20, '0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01;0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (96, 'EL03', 'BOAT_HAMINA', 15000, 20, '0,0:FA01-0,0:FA01;6,50:FF02-0,0:FA01;4,999:FI05-0,0:FA01;999,0:FB04-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (97, 'EM01', 'BOAT_MKV', 4000, 20, '0,0:FA01-0,0:FA01;10,10:FH01-0,0:FA01;30000,0:FB01-0,0:FA01;30000,0:FB01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (98, 'EM02', 'BOAT_PWC', 3000, 20, '0,0:FA01-0,0:FA01', '1');
INSERT INTO `vehicles` VALUES (99, 'EM03', 'BOAT_CRRC', 0, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (100, 'EN01', 'FIXED_BRK_DRUM', 1000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (101, 'EN02', 'FIXED_BRK_DOOR', 300000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (102, 'EN03', 'FIXED_BRK_ENGINE', 600000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (103, 'EN04', 'FIXED_BRK_DOOR2', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (104, 'EN05', 'FIXED_BRK_CONTROL_UNIT', 1000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (105, 'EN06', 'FIXED_BRK_MISSILE', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (106, 'EN07', 'FIXED_BRK_DOOR3', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (107, 'EN08', 'FIXED_BRK_DOOR4', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (108, 'EN09', 'FIXED_BRK_DOOR5', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (109, 'EN10', 'FIXED_BRK_CONTROL_UNIT2', 1500000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (110, 'EN11', 'FIXED_BRK_RADAR', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (111, 'EN12', 'FIXED_BRK_MISSILE2', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (112, 'EN13', 'FIXED_BRK_DOOR6', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (113, 'EN14', 'FIXED_BRK_DP05_DER', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (114, 'EN15', 'FIXED_BRK_DP05_NIU', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (115, 'EN16', 'FIXED_BRK_INCUBATOR', 100000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (116, 'EN17', 'FIXED_BRK_PRISON_GATE', 50000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (117, 'EN18', 'FIXED_BRK_PRISON_DOOR', 200000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (118, 'EN19', 'FIXED_BRK_SAFE_DOOR', 80000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (119, 'EN20', 'FIXED_BRK_PRISON_EndDR', 80000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (120, 'EN21', 'FIXED_BRK_PRISON_EndDL', 80000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (121, 'EN22', 'FIXED_BRK_STARTDOOR1', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (122, 'EN23', 'FIXED_BRK_STARTDOOR2', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (123, 'EN24', 'FIXED_BRK_HACKDOOR1', 9000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (124, 'EN25', 'FIXED_BRK_HACKDOOR2', 12000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (125, 'EN26', 'FIXED_BRK_FIRE_WALL', 14000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (126, 'EN27', 'FIXED_BRK_OIL_TANK', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (127, 'EN28', 'FIXED_BRK_JAIL1', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (128, 'EN29', 'FIXED_BRK_JAIL2', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (129, 'EN30', 'FIXED_BRK_MANHOLE', 3000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (130, 'EN31', 'FIXED_BRK_ELEVATOR_LOOP', 3000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (131, 'EN32', 'FIXED_BRK_BOOM', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (132, 'EN33', 'FIXED_BRK_HACKDOOR3', 25000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (133, 'EN34', 'FIXED_BRK_HACKDOOR4', 16000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (134, 'EN35', 'FIXED_BRK_PRISON_Mujun', 90000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (135, 'EN36', 'FIXED_BRK_BOARD1', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (136, 'EN37', 'FIXED_BRK_BOARD2', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (137, 'EN38', 'FIXED_BRK_BOARD3', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (138, 'EN39', 'FIXED_BRK_BOARD4', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (139, 'EN40', 'FIXED_BRK_BOARD5', 8000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (140, 'EN41', 'FIXED_BRK_MANHOLE2', 5000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (141, 'EN42', 'FIXED_BRK_ELEVATOR_DOOR', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (142, 'EN43', 'FIXED_BRK_EN43_DER', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (143, 'EN44', 'FIXED_BRK_HACKBRIDGE', 250000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (144, 'EN45', 'FIXED_BRK_FENCE', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (145, 'EN46', 'FIXED_BRK_HACKDOOR5', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (146, 'EN47', 'FIXED_BRK_HACKDOOR6', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (147, 'EN48', 'FIXED_BRK_HACKBRIDGE1', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (148, 'EN49', 'FIXED_BRK_HACKDOOR7', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (149, 'EN50', 'FIXED_BRK_CAR', 10000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (150, 'EN51', 'FIXED_BRK_SHUTTER', 5000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (151, 'EN52', 'FIXED_BRK_PRISON_GATE2', 250000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (152, 'EN53', 'FIXED_BRK_S1_Cdoor', 500000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (153, 'EN54', 'FIXED_BRK_S4_BossDoor', 4000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (154, 'EN55', 'FIXED_BRK_S2_Cdoor', 80000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (155, 'EN56', 'FIXED_BRK_BIGFAN', 10000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (156, 'EN57', 'FIXED_BRK_BIGFAN2', 10000000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (157, 'EN58', 'DirectionSign', 0, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (158, 'EN59', 'FIXED_BRK_PRESENTBOX', 21000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (159, 'EN60', 'FIXED_BRK_TW_Gate1', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (160, 'EN61', 'FIXED_BRK_TW_Gate2', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (161, 'EN62', 'FIXED_BRK_TW_Gate3', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (162, 'EN63', 'FIXED_BRK_TW_Gate4', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (163, 'EN64', 'FIXED_BRK_TW_Gate5', 15000, 20, '0,0:NULL-0,0:NULL', '0');
INSERT INTO `vehicles` VALUES (164, 'EN65', 'FIXED_BRK_PM_Wall', 999999, 20, '0,0:NULL-0,0:NULL', '0');

-- ----------------------------
-- Table structure for web_partnerek
-- ----------------------------
DROP TABLE IF EXISTS `web_partnerek`;
CREATE TABLE `web_partnerek`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(100) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `url` text CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `kepurl` text CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `kiemelt` enum('igen','nem') CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL DEFAULT 'nem',
  `ekkor` datetime(0) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_hungarian_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for web_targypiac
-- ----------------------------
DROP TABLE IF EXISTS `web_targypiac`;
CREATE TABLE `web_targypiac`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ar` int(11) NOT NULL,
  `valuta` enum('wrpenz','dinar','erme') CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL DEFAULT 'wrpenz',
  `nev` varchar(50) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `kod` char(4) CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `kep` mediumtext CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `aktiv` enum('0','1') CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL DEFAULT '1',
  `nap` int(11) NOT NULL,
  `leiras` text CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `informacio` text CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL,
  `tarazas` int(11) NOT NULL DEFAULT 0,
  `pontossag` int(11) NOT NULL DEFAULT 0,
  `sebesseg` int(11) NOT NULL DEFAULT 0,
  `sebzes` int(11) NOT NULL DEFAULT 0,
  `kategoria` enum('fegyver','felszereles','karakter','targy') CHARACTER SET utf8 COLLATE utf8_hungarian_ci NOT NULL DEFAULT 'fegyver',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_hungarian_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for wordfilter
-- ----------------------------
DROP TABLE IF EXISTS `wordfilter`;
CREATE TABLE `wordfilter`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `normal` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `replace` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Table structure for zombies
-- ----------------------------
DROP TABLE IF EXISTS `zombies`;
CREATE TABLE `zombies`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` int(11) NOT NULL DEFAULT 0,
  `name` char(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  `health` int(11) NOT NULL DEFAULT 0,
  `points` int(11) NOT NULL DEFAULT 0,
  `damage` int(11) NOT NULL DEFAULT 0,
  `skillpoint` enum('0','1') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = MyISAM AUTO_INCREMENT = 24 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Fixed;

-- ----------------------------
-- Records of zombies
-- ----------------------------
INSERT INTO `zombies` VALUES (1, 0, 'Madman', 1, 1, 150, '0');
INSERT INTO `zombies` VALUES (2, 1, 'Maniac', 2, 1, 150, '0');
INSERT INTO `zombies` VALUES (3, 2, 'Grinder', 100, 2, 200, '0');
INSERT INTO `zombies` VALUES (4, 3, 'Grounder', 111, 2, 200, '0');
INSERT INTO `zombies` VALUES (5, 4, ' Growler', 125, 2, 250, '0');
INSERT INTO `zombies` VALUES (6, 5, 'Heavy', 9000, 10, 450, '0');
INSERT INTO `zombies` VALUES (7, 6, 'Lover', 150, 3, 800, '0');
INSERT INTO `zombies` VALUES (8, 7, 'Handgeman', 133, 4, 300, '1');
INSERT INTO `zombies` VALUES (9, 8, 'Chariot', 20000, 50, 500, '0');
INSERT INTO `zombies` VALUES (10, 9, 'Crusher', 40000, 100, 650, '0');
INSERT INTO `zombies` VALUES (11, 10, 'Buster', 45000, 150, 700, '0');
INSERT INTO `zombies` VALUES (12, 11, 'Crasher', 10000, 200, 1000, '0');
INSERT INTO `zombies` VALUES (13, 12, 'Envy', 150, 5, 500, '0');
INSERT INTO `zombies` VALUES (14, 13, 'Claw', 5, 3, 200, '0');
INSERT INTO `zombies` VALUES (15, 14, 'Bomber', 30000, 50, 1000, '0');
INSERT INTO `zombies` VALUES (16, 15, 'Defender', 300000, 100, 1000, '0');
INSERT INTO `zombies` VALUES (17, 16, 'Breaker', 350000, 200, 1000, '0');
INSERT INTO `zombies` VALUES (18, 17, 'MadSoldier', 2, 2, 360, '0');
INSERT INTO `zombies` VALUES (19, 18, 'MadPrisoner', 1, 1, 240, '0');
INSERT INTO `zombies` VALUES (20, 19, 'Breaker2', 350000, 100, 1000, '0');
INSERT INTO `zombies` VALUES (21, 20, 'SuperHeavy', 10000, 15, 500, '0');
INSERT INTO `zombies` VALUES (22, 21, 'Lady', 5, 5, 500, '0');
INSERT INTO `zombies` VALUES (23, 22, 'Midget', 5, 5, 150, '0');

SET FOREIGN_KEY_CHECKS = 1;
