DROP TABLE IF EXISTS friendsofaward_juryrating;
DROP TABLE IF EXISTS friendsofaward_rating;
DROP TABLE IF EXISTS friendsofaward_user;
DROP TABLE IF EXISTS friendsofaward_projects;
DROP TABLE IF EXISTS friendsofaward_admin;

CREATE TABLE friendsofaward_admin (
    user_email VARCHAR(255) PRIMARY KEY,
    password_admin VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

INSERT INTO friendsofaward_admin (user_email, password_admin)
VALUES ('Admin','5G5ylso3zI2UBwdSvAZK+g==:fIJiMbwxtF5VsV9NofhUhFyfMn6dULmxrV65p/b23ZY=');

CREATE TABLE friendsofaward_projects (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    author VARCHAR(255) NOT NULL,
    schulvoting INT NOT NULL DEFAULT 0,
    publikumsvoting INT NOT NULL DEFAULT 0,
    juryvoting INT NOT NULL DEFAULT 0
) ENGINE=InnoDB;

INSERT INTO friendsofaward_projects (title, author) VALUES
('Titel1','Autor1'),
('Titel2','Autor2'),
('Titel3','Autor3'),
('Titel4','Autor4'),
('Titel5','Autor5'),
('Titel6','Autor6'),
('Titel7','Autor7'),
('Titel8','Autor8'),
('Titel9','Autor9'),
('Titel10','Autor10');

CREATE TABLE friendsofaward_user (
    token VARCHAR(255) NOT NULL,
    tokenused TINYINT(1) NOT NULL,
    tokentype VARCHAR(50) NOT NULL,
    PRIMARY KEY (token)
) ENGINE=InnoDB;

CREATE TABLE friendsofaward_rating (
    token VARCHAR(255) NOT NULL,
    topfavorit INT NOT NULL DEFAULT 0,
    favorit1 INT NOT NULL DEFAULT 0,
    favorit2 INT NOT NULL DEFAULT 0,
    favorit3 INT NOT NULL DEFAULT 0,
    favorit4 INT NOT NULL DEFAULT 0,
    favorit5 INT NOT NULL DEFAULT 0,
    PRIMARY KEY (token),
    CONSTRAINT FK_rating_token
        FOREIGN KEY (token)
        REFERENCES friendsofaward_user (token)
) ENGINE=InnoDB;

CREATE TABLE friendsofaward_juryrating (
    token VARCHAR(255) NOT NULL,
    projekt1 INT NOT NULL DEFAULT 0,
    projekt2 INT NOT NULL DEFAULT 0,
    projekt3 INT NOT NULL DEFAULT 0,
    projekt4 INT NOT NULL DEFAULT 0,
    projekt5 INT NOT NULL DEFAULT 0,
    projekt6 INT NOT NULL DEFAULT 0,
    PRIMARY KEY (token),
    CONSTRAINT FK_jury_token
        FOREIGN KEY (token)
        REFERENCES friendsofaward_user (token)
) ENGINE=InnoDB;
