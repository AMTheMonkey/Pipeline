#!/bin/bash
# $1 is the file name $2 is the limit

line_code_coverage=$(cat $1 | grep "Line coverage")
code_coverage="$(echo $line_code_coverage | grep -o -E '[0-9]+\.?([0-9]+)?')"
limit=$2
red="\033[0;31m"
green="\033[0;32m"
no_color="\033[0m"

if (( $(echo "$code_coverage<$2" | bc -l) ))
then
	echo -e "${red}$line_code_coverage (expected at least $2)${no_color}"
	exit 1
fi

echo -e "${green}$line_code_coverage${no_color}"
exit 0

