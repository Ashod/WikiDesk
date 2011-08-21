FOR /R Mediawiki/languages/messages %%i IN (message*.php) DO phpc /target:dll Mediawiki/languages/messages/%%~ni.php /out:bin/MediaWiki.Net.Languages.Messages.%%~ni.dll


phpc /target:dll /root:. /recurse:Mediawiki/languages/ /skip:Mediawiki/languages/messages/ /out:bin/MediaWiki.Net.Languages.dll
phpc /target:dll /root:. /recurse:Mediawiki/includes/parser /out:bin/MediaWiki.Net.Includes.Parser.dll

phpc /target:dll /root:. /recurse:Mediawiki/includes/parser /out:bin/MediaWiki.Net.Includes.Parser.dll

phpc /target:dll /root:. /recurse:Mediawiki/includes/ /skip:Mediawiki/includes/installer/ /skip:Mediawiki/includes/api/ /skip:Mediawiki/includes/specials/ /skip:Mediawiki/includes/upload/ /skip:Mediawiki/includes/zhtable/ /skip:Mediawiki/includes/ForkController.php /skip:Mediawiki/includes/Skin.php /out:bin/MediaWiki.Net.Inc.dll


@rem phpc /target:dll /root:. /recurse:. /skip:Mediawiki/languages/messages/ /skip:Mediawiki/maintenance/ /skip:Mediawiki/includes/ForkController.php /skip:Mediawiki/includes/Skin.php /out:bin/MW.dll