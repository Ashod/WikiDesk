<?php
/**
 * @defgroup Language Language
 *
 * @file
 * @ingroup Language
 */

if( !defined( 'MEDIAWIKI' ) ) {
	echo "This file is part of MediaWiki, it is not a valid entry point.\n";
	exit( 1 );
}

# Read language names
global $wgLanguageNames;
require_once( dirname(__FILE__) . '/Names.php' ) ;

global $wgInputEncoding, $wgOutputEncoding;

/**
 * These are always UTF-8, they exist only for backwards compatibility
 */
$wgInputEncoding    = "UTF-8";
$wgOutputEncoding	= "UTF-8";

if( function_exists( 'mb_strtoupper' ) ) {
	mb_internal_encoding('UTF-8');
}

/**
 * a fake language converter
 *
 * @ingroup Language
 */
class FakeConverter {
	var $mLang;
	function FakeConverter( $langobj ) { $this->mLang = $langobj; }
	function autoConvertToAllVariants( $text ) { return $text; }
	function convert( $t ) { return $t; }
	function convertTitle( $t ) { return $t->getPrefixedText(); }
	function getVariants() { return array( $this->mLang->getCode() ); }
	function getPreferredVariant() { return $this->mLang->getCode(); }
	function getConvRuleTitle() { return false; }
	function findVariantLink(&$l, &$n, $ignoreOtherCond = false) {}
	function getExtraHashOptions() {return '';}
	function getParsedTitle() {return '';}
	function markNoConversion($text, $noParse=false) {return $text;}
	function convertCategoryKey( $key ) {return $key; }
	function convertLinkToAllVariants($text){ return array( $this->mLang->getCode() => $text); }
	function armourMath($text){ return $text; }
}

/**
 * Internationalisation code
 * @ingroup Language
 */
class Language {
	var $mConverter, $mVariants, $mCode, $mLoaded = false;
	var $mMagicExtensions = array(), $mMagicHookDone = false;

	var $mNamespaceIds, $namespaceNames, $namespaceAliases;
	var $dateFormatStrings = array();
	var $mExtendedSpecialPageAliases;

	/**
	 * ReplacementArray object caches
	 */
	var $transformData = array();

	static public $dataCache;
	static public $mLangObjCache = array();

	static public $mWeekdayMsgs = array(
		'sunday', 'monday', 'tuesday', 'wednesday', 'thursday',
		'friday', 'saturday'
	);

	/**
	 * Provides an alternative text depending on specified gender.
	 * Usage {{gender:username|masculine|feminine|neutral}}.
	 * username is optional, in which case the gender of current user is used,
	 * but only in (some) interface messages; otherwise default gender is used.
	 * If second or third parameter are not specified, masculine is used.
	 * These details may be overriden per language.
	 */
	function gender( $gender, $forms ) {
		if ( !count($forms) ) { return ''; }
		$forms = $this->preConvertPlural( $forms, 2 );
		if ( $gender === 'male' ) return $forms[0];
		if ( $gender === 'female' ) return $forms[1];
		return isset($forms[2]) ? $forms[2] : $forms[0];
	}

	/**
	 * Get the conversion rule title, if any.
	 */
	function getConvRuleTitle() {
		return $this->mConverter->getConvRuleTitle();
	}
}
