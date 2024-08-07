-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        11.4.2-MariaDB - mariadb.org binary distribution
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- game 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `game` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `game`;

-- 테이블 game.users 구조 내보내기
CREATE TABLE IF NOT EXISTS `users` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(50) NOT NULL,
  `pw` varchar(100) NOT NULL,
  `name` varchar(50) DEFAULT NULL,
  `LEVEL` int(11) DEFAULT NULL,
  `class` int(11) unsigned zerofill NOT NULL,
  `profile_text` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`uid`),
  UNIQUE KEY `UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 game.users:~8 rows (대략적) 내보내기
INSERT INTO `users` (`uid`, `email`, `pw`, `name`, `LEVEL`, `class`, `profile_text`) VALUES
	(1, 'abc@abc.abc', '1234', '헿', 5, 00000000000, '안녕하세요.'),
	(4, 'ssb9292@naver.com', '123456789', '신승범', 31, 00000000000, '반가워요'),
	(5, 'ssb9292@gmail.com', '2345', 'ㅁ', 10, 00000000000, '나는 10레벨 입니다.'),
	(6, 'aaa@aaa.aaa', 'aaaa', 'ㄴ', 5, 00000000000, '내 이메일은 a로만 이루어져 있지'),
	(7, 'bbb@naver.com', 'bbbbb', 'ㅇ', 5, 00000000000, '내 패스워드는 b로만 이루어져 있지'),
	(8, 'happy@naver.com', 'happy', 'ㄹ', 6, 00000000000, '행복'),
	(9, 'bcd@bcd.bcd', '1234', NULL, 1, 00000000000, NULL),
	(11, '123@123.com', '03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4', '신승범', 35, 00000000002, '저는 유석호입니다.');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
