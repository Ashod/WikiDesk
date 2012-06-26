<?php
/**
 * This does the initial setup for the WikiDesk version.
 * It sets up configuration and optionally loads Setup.php
 * depending on whether MW_NO_SETUP is defined.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 * http://www.gnu.org/copyleft/gpl.html
 *
 * @file
 */

$wgRequestTime = microtime(true);
# getrusage() does not exist on the Microsoft Windows platforms, catching this
if ( function_exists ( 'getrusage' ) ) {
	$wgRUstart = getrusage();
} else {
	$wgRUstart = array();
}
unset( $IP );

# Valid web server entry point, enable includes.
# Please don't move this line to includes/Defines.php. This line essentially
# defines a valid entry point. If you put it in includes/Defines.php, then
# any script that includes it becomes an entry point, thereby defeating
# its purpose.
define( 'MEDIAWIKI', true );

# Full path to working directory.
# Makes it possible to for example to have effective exclude path in apc.
# Also doesn't break installations using symlinked includes, like
# dirname( __FILE__ ) would do.
$IP = getenv( 'MW_INSTALL_PATH' );
if ( $IP === false ) {
	$IP = realpath( '.' );
}

if ( isset( $_SERVER['MW_COMPILED'] ) ) {
	define( 'MW_COMPILED', 1 );
} else {
	# Get MWInit class
	require_once( "$IP/includes/Init.php" );

	# Start the autoloader, so that extensions can derive classes from core files
	require_once( "$IP/includes/AutoLoader.php" );

	# Load the profiler
	require_once( "$IP/includes/profiler/Profiler.php" );

	# Load up some global defines.
	require_once( "$IP/includes/Defines.php" );
}

# Load default settings
require_once( MWInit::compiledPath( "includes/DefaultSettings.php" ) );

# Load default settings
require_once( MWInit::compiledPath( "LocalSettings.php" ) );

if ( !defined( 'MW_NO_SETUP' ) ) {
	require_once( MWInit::compiledPath( "includes/Setup.php" ) );
}

