# WikiDesk #

Wikipedia browser supporting all flavors and languages with auto-update.

WikiDesk stores the wiki-code in a local database and renders the pages on-demand. It has an index and full-text search.
Unlike other Wikipedia readers, this isn't an offline reader. It's an online reader with offline backup and caching. It could serve as an offline reader if the DB is stocked with data and online access disabled. However, the main purpose of the project is to give the user an up-to-date access to wiki entries, with rich multimedia experience, while reducing the load on the wiki servers, improving user experience with fast browsing and providing an offline browsable backup.

## Build ##

1. Download and install Phalanger (http://www.php-compiler.net/).
1. Open project with VS2008.
1. Build.

## Installation ##

1. Only manual build is supported for now.

## Goals ##

### Primary: ###

* A stand-alone, portable WikiMedia markup parser.
* Offline reader with on-demand update and customized database.
* Support for all wiki domains and languages.

### Secondary: ###

* Promote language-learning.
* Encourage the creation and enhancement of entries in multiple languages.
* Promote more participation.

### Non-Goals/Out of scope: ###

* Exact reproduction of wikipedia.
* Backup of revisions (only one revision per entry is stored.)
* User- or account-specific functionality.
* Live wiki server specific functionality.

### Changelog ###

#### 0.2 ####
* First public version (revision 252).
* Alpha product - use at your own risk.