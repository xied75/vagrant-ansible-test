#!/bin/bash

function webCall {
  for i in `seq 1 100`;
    do
      host=$(curl -s localhost:80 | grep -o "With love from \w*" | cut -d' ' -f4)
    echo $host
  done
}

echo "Testing loadbalancer... (100 web call)"

echo "  sleep 3 seconds for the last host to warm up"

sleep 3

webCall | sort | uniq -c

echo "You should see near equal number for each host"
