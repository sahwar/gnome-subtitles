## intltool.m4 - Configure intltool for the target system. -*-Shell-script-*-
## Copyright (C) 2001 Eazel, Inc.
## Author: Maciej Stachowiak <mjs@noisehavoc.org>
##         Kenneth Christiansen <kenneth@gnu.org>
##
## This program is free software; you can redistribute it and/or modify
## it under the terms of the GNU General Public License as published by
## the Free Software Foundation; either version 2 of the License, or
## (at your option) any later version.
##
## This program is distributed in the hope that it will be useful, but
## WITHOUT ANY WARRANTY; without even the implied warranty of
## MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
## General Public License for more details.
##
## You should have received a copy of the GNU General Public License
## along with this program; if not, see <http://www.gnu.org/licenses/>.
##
## As a special exception to the GNU General Public License, if you
## distribute this file as part of a program that contains a
## configuration script generated by Autoconf, you may include it under
## the same distribution terms that you use for the rest of that program.

dnl IT_PROG_INTLTOOL([MINIMUM-VERSION], [no-xml])
# serial 42 IT_PROG_INTLTOOL
AC_DEFUN([IT_PROG_INTLTOOL], [
AC_PREREQ([2.50])dnl
AC_REQUIRE([AM_NLS])dnl

case "$am__api_version" in
    1.[01234])
	AC_MSG_ERROR([Automake 1.5 or newer is required to use intltool])
    ;;
    *)
    ;;
esac

INTLTOOL_REQUIRED_VERSION_AS_INT=`echo $1 | awk -F. '{ print $ 1 * 1000 + $ 2 * 100 + $ 3; }'`
INTLTOOL_APPLIED_VERSION=`intltool-update --version | head -1 | cut -d" " -f3`
INTLTOOL_APPLIED_VERSION_AS_INT=`echo $INTLTOOL_APPLIED_VERSION | awk -F. '{ print $ 1 * 1000 + $ 2 * 100 + $ 3; }'`
if test -n "$1"; then
    AC_MSG_CHECKING([for intltool >= $1])
    AC_MSG_RESULT([$INTLTOOL_APPLIED_VERSION found])
    test "$INTLTOOL_APPLIED_VERSION_AS_INT" -ge "$INTLTOOL_REQUIRED_VERSION_AS_INT" ||
	AC_MSG_ERROR([Your intltool is too old.  You need intltool $1 or later.])
fi

AC_PATH_PROG(INTLTOOL_UPDATE, [intltool-update])
AC_PATH_PROG(INTLTOOL_MERGE, [intltool-merge])
AC_PATH_PROG(INTLTOOL_EXTRACT, [intltool-extract])
if test -z "$INTLTOOL_UPDATE" -o -z "$INTLTOOL_MERGE" -o -z "$INTLTOOL_EXTRACT"; then
    AC_MSG_ERROR([The intltool scripts were not found. Please install intltool.])
fi

if test -z "$AM_DEFAULT_VERBOSITY"; then
  AM_DEFAULT_VERBOSITY=1
fi
AC_SUBST([AM_DEFAULT_VERBOSITY])

INTLTOOL_V_MERGE='$(INTLTOOL__v_MERGE_$(V))'
INTLTOOL__v_MERGE_='$(INTLTOOL__v_MERGE_$(AM_DEFAULT_VERBOSITY))'
INTLTOOL__v_MERGE_0='@echo "  ITMRG " [$]@;'
AC_SUBST(INTLTOOL_V_MERGE)
AC_SUBST(INTLTOOL__v_MERGE_)
AC_SUBST(INTLTOOL__v_MERGE_0)

INTLTOOL_V_MERGE_OPTIONS='$(intltool__v_merge_options_$(V))'
intltool__v_merge_options_='$(intltool__v_merge_options_$(AM_DEFAULT_VERBOSITY))'
intltool__v_merge_options_0='-q'
AC_SUBST(INTLTOOL_V_MERGE_OPTIONS)
AC_SUBST(intltool__v_merge_options_)
AC_SUBST(intltool__v_merge_options_0)

  INTLTOOL_DESKTOP_RULE='%.desktop:   %.desktop.in   $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
INTLTOOL_DIRECTORY_RULE='%.directory: %.directory.in $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
     INTLTOOL_KEYS_RULE='%.keys:      %.keys.in      $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -k -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
     INTLTOOL_PROP_RULE='%.prop:      %.prop.in      $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
      INTLTOOL_OAF_RULE='%.oaf:       %.oaf.in       $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -o -p $(top_srcdir)/po $< [$]@'
     INTLTOOL_PONG_RULE='%.pong:      %.pong.in      $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
   INTLTOOL_SERVER_RULE='%.server:    %.server.in    $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -o -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
    INTLTOOL_SHEET_RULE='%.sheet:     %.sheet.in     $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
INTLTOOL_SOUNDLIST_RULE='%.soundlist: %.soundlist.in $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
       INTLTOOL_UI_RULE='%.ui:        %.ui.in        $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
      INTLTOOL_XML_RULE='%.xml:       %.xml.in       $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
if test "$INTLTOOL_APPLIED_VERSION_AS_INT" -ge 5000; then
      INTLTOOL_XML_NOMERGE_RULE='%.xml:       %.xml.in       $(INTLTOOL_MERGE) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u --no-translations $< [$]@'
else
      INTLTOOL_XML_NOMERGE_RULE='%.xml:       %.xml.in       $(INTLTOOL_MERGE) ; $(INTLTOOL_V_MERGE)_it_tmp_dir=tmp.intltool.[$][$]RANDOM && mkdir [$][$]_it_tmp_dir && LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u [$][$]_it_tmp_dir $< [$]@ && rmdir [$][$]_it_tmp_dir'
fi
      INTLTOOL_XAM_RULE='%.xam:       %.xml.in       $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
      INTLTOOL_KBD_RULE='%.kbd:       %.kbd.in       $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -m -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
    INTLTOOL_CAVES_RULE='%.caves:     %.caves.in     $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
  INTLTOOL_SCHEMAS_RULE='%.schemas:   %.schemas.in   $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -s -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
    INTLTOOL_THEME_RULE='%.theme:     %.theme.in     $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@' 
    INTLTOOL_SERVICE_RULE='%.service: %.service.in   $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -d -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@'
   INTLTOOL_POLICY_RULE='%.policy:    %.policy.in    $(INTLTOOL_MERGE) $(wildcard $(top_srcdir)/po/*.po) ; $(INTLTOOL_V_MERGE)LC_ALL=C $(INTLTOOL_MERGE) $(INTLTOOL_V_MERGE_OPTIONS) -x -u -c $(top_builddir)/po/.intltool-merge-cache $(top_srcdir)/po $< [$]@'

_IT_SUBST(INTLTOOL_DESKTOP_RULE)
_IT_SUBST(INTLTOOL_DIRECTORY_RULE)
_IT_SUBST(INTLTOOL_KEYS_RULE)
_IT_SUBST(INTLTOOL_PROP_RULE)
_IT_SUBST(INTLTOOL_OAF_RULE)
_IT_SUBST(INTLTOOL_PONG_RULE)
_IT_SUBST(INTLTOOL_SERVER_RULE)
_IT_SUBST(INTLTOOL_SHEET_RULE)
_IT_SUBST(INTLTOOL_SOUNDLIST_RULE)
_IT_SUBST(INTLTOOL_UI_RULE)
_IT_SUBST(INTLTOOL_XAM_RULE)
_IT_SUBST(INTLTOOL_KBD_RULE)
_IT_SUBST(INTLTOOL_XML_RULE)
_IT_SUBST(INTLTOOL_XML_NOMERGE_RULE)
_IT_SUBST(INTLTOOL_CAVES_RULE)
_IT_SUBST(INTLTOOL_SCHEMAS_RULE)
_IT_SUBST(INTLTOOL_THEME_RULE)
_IT_SUBST(INTLTOOL_SERVICE_RULE)
_IT_SUBST(INTLTOOL_POLICY_RULE)

# Check the gettext tools to make sure they are GNU
AC_PATH_PROG(XGETTEXT, xgettext)
AC_PATH_PROG(MSGMERGE, msgmerge)
AC_PATH_PROG(MSGFMT, msgfmt)
AC_PATH_PROG(GMSGFMT, gmsgfmt, $MSGFMT)
if test -z "$XGETTEXT" -o -z "$MSGMERGE" -o -z "$MSGFMT"; then
    AC_MSG_ERROR([GNU gettext tools not found; required for intltool])
fi
xgversion="`$XGETTEXT --version|grep '(GNU ' 2> /dev/null`"
mmversion="`$MSGMERGE --version|grep '(GNU ' 2> /dev/null`"
mfversion="`$MSGFMT --version|grep '(GNU ' 2> /dev/null`"
if test -z "$xgversion" -o -z "$mmversion" -o -z "$mfversion"; then
    AC_MSG_ERROR([GNU gettext tools not found; required for intltool])
fi

AC_PATH_PROG(INTLTOOL_PERL, perl)
if test -z "$INTLTOOL_PERL"; then
   AC_MSG_ERROR([perl not found])
fi
AC_MSG_CHECKING([for perl >= 5.8.1])
$INTLTOOL_PERL -e "use 5.8.1;" > /dev/null 2>&1
if test $? -ne 0; then
   AC_MSG_ERROR([perl 5.8.1 is required for intltool])
else
   IT_PERL_VERSION=`$INTLTOOL_PERL -e "printf '%vd', $^V"`
   AC_MSG_RESULT([$IT_PERL_VERSION])
fi
if test "x$2" != "xno-xml"; then
   AC_MSG_CHECKING([for XML::Parser])
   if `$INTLTOOL_PERL -e "require XML::Parser" 2>/dev/null`; then
       AC_MSG_RESULT([ok])
   else
       AC_MSG_ERROR([XML::Parser perl module is required for intltool])
   fi
fi

# Substitute ALL_LINGUAS so we can use it in po/Makefile
AC_SUBST(ALL_LINGUAS)

# Set DATADIRNAME correctly if it is not set yet
# (copied from glib-gettext.m4)
if test -z "$DATADIRNAME"; then
  AC_LINK_IFELSE(
    [AC_LANG_PROGRAM([[]],
                     [[extern int _nl_msg_cat_cntr;
                       return _nl_msg_cat_cntr]])],
    [DATADIRNAME=share],
    [case $host in
    *-*-solaris*)
    dnl On Solaris, if bind_textdomain_codeset is in libc,
    dnl GNU format message catalog is always supported,
    dnl since both are added to the libc all together.
    dnl Hence, we'd like to go with DATADIRNAME=share
    dnl in this case.
    AC_CHECK_FUNC(bind_textdomain_codeset,
      [DATADIRNAME=share], [DATADIRNAME=lib])
    ;;
    *)
    [DATADIRNAME=lib]
    ;;
    esac])
fi
AC_SUBST(DATADIRNAME)

IT_PO_SUBDIR([po])

])


# IT_PO_SUBDIR(DIRNAME)
# ---------------------
# All po subdirs have to be declared with this macro; the subdir "po" is
# declared by IT_PROG_INTLTOOL.
#
AC_DEFUN([IT_PO_SUBDIR],
[AC_PREREQ([2.53])dnl We use ac_top_srcdir inside AC_CONFIG_COMMANDS.
dnl
dnl The following CONFIG_COMMANDS should be executed at the very end
dnl of config.status.
AC_CONFIG_COMMANDS_PRE([
  AC_CONFIG_COMMANDS([$1/stamp-it], [
    if [ ! grep "^# INTLTOOL_MAKEFILE$" "$1/Makefile.in" > /dev/null ]; then
       AC_MSG_ERROR([$1/Makefile.in.in was not created by intltoolize.])
    fi
    rm -f "$1/stamp-it" "$1/stamp-it.tmp" "$1/POTFILES" "$1/Makefile.tmp"
    >"$1/stamp-it.tmp"
    [sed '/^#/d
	 s/^[[].*] *//
	 /^[ 	]*$/d
	'"s|^|	$ac_top_srcdir/|" \
      "$srcdir/$1/POTFILES.in" | sed '$!s/$/ \\/' >"$1/POTFILES"
    ]
    [sed '/^POTFILES =/,/[^\\]$/ {
		/^POTFILES =/!d
		r $1/POTFILES
	  }
	 ' "$1/Makefile.in" >"$1/Makefile"]
    rm -f "$1/Makefile.tmp"
    mv "$1/stamp-it.tmp" "$1/stamp-it"
  ])
])dnl
])

# _IT_SUBST(VARIABLE)
# -------------------
# Abstract macro to do either _AM_SUBST_NOTMAKE or AC_SUBST
#
AC_DEFUN([_IT_SUBST],
[
AC_SUBST([$1])
m4_ifdef([_AM_SUBST_NOTMAKE], [_AM_SUBST_NOTMAKE([$1])])
]
)

# deprecated macros
AU_ALIAS([AC_PROG_INTLTOOL], [IT_PROG_INTLTOOL])
# A hint is needed for aclocal from Automake <= 1.9.4:
# AC_DEFUN([AC_PROG_INTLTOOL], ...)

