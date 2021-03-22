-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        8.0.19 - MySQL Community Server - GPL
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- 导出 snsdb 的数据库结构
CREATE DATABASE IF NOT EXISTS `snsdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `snsdb`;

-- 导出  表 snsdb.account 结构
CREATE TABLE IF NOT EXISTS `account` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Account` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '账号',
  `Pwd` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '密码',
  `NickName` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '昵称，最大八个汉字',
  `Avatar` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '头像路径',
  `Intro` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '一句话介绍，最大150个中文，中文占3长度',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Account` (`Account`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='用户表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.article 结构
CREATE TABLE IF NOT EXISTS `article` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL COMMENT '用户ID',
  `Title` varchar(100) COLLATE utf8mb4_general_ci NOT NULL COMMENT '标题',
  `ContentType` int NOT NULL COMMENT '内容类型 1：文章',
  `ViewCount` int NOT NULL COMMENT '阅读数',
  `CreateTime` datetime NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_Article_AccountId` (`AccountId`),
  KEY `IDX_Article_ContentType` (`ContentType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='文章表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.article_content 结构
CREATE TABLE IF NOT EXISTS `article_content` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ArticleId` int NOT NULL COMMENT '文章ID',
  `Body` varchar(3000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '内容体3000个字符长度',
  PRIMARY KEY (`Id`),
  KEY `IDK_Content_ArticleId` (`ArticleId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='文章内容表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.article_topic 结构
CREATE TABLE IF NOT EXISTS `article_topic` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ArticleId` int NOT NULL COMMENT '文章ID',
  `TopicId` int NOT NULL COMMENT '话题ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_ArticleId_TopicId` (`ArticleId`,`TopicId`),
  KEY `IDX_ArticleTopic_TopicId` (`TopicId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 数据导出被取消选择。

-- 导出  表 snsdb.comment 结构
CREATE TABLE IF NOT EXISTS `comment` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL COMMENT '用户ID',
  `QuoteId` int NOT NULL COMMENT '引用ID',
  `ArticleId` int NOT NULL COMMENT '文章ID',
  `Body` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '评论内容',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_Comment_ArticleId` (`ArticleId`),
  KEY `IDX_Comment_AccountId_ArticleId` (`AccountId`,`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='评论表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.comment_thumb 结构
CREATE TABLE IF NOT EXISTS `comment_thumb` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL COMMENT '用户ID',
  `CommentId` int NOT NULL COMMENT '评论ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_CommentThumb_AccountId_CommentId` (`AccountId`,`CommentId`),
  KEY `IDX_CommentThumb_CommentId` (`CommentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='评论赞表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.favorite 结构
CREATE TABLE IF NOT EXISTS `favorite` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL COMMENT '用户ID',
  `ArticleId` int NOT NULL COMMENT '文章ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Favorite_AccountId_ArticleId` (`AccountId`,`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='收藏表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.feedback 结构
CREATE TABLE IF NOT EXISTS `feedback` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '名字',
  `Contact` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '联系方式',
  `Content` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '内容',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='反馈表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.follow 结构
CREATE TABLE IF NOT EXISTS `follow` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL COMMENT '关注者ID',
  `FollowId` int NOT NULL COMMENT '被关注ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Follow_AccountId_FollowId` (`AccountId`,`FollowId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='关注表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.thumb 结构
CREATE TABLE IF NOT EXISTS `thumb` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL COMMENT '用户ID',
  `ArticleId` int NOT NULL COMMENT '文章ID',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Thumb_AccountId_ArticleId` (`AccountId`,`ArticleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='点赞表';

-- 数据导出被取消选择。

-- 导出  表 snsdb.topic 结构
CREATE TABLE IF NOT EXISTS `topic` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '名称',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UK_Topic_Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='话题表';

-- 数据导出被取消选择。

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
