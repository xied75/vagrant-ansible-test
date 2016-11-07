## How to run

1. Clone this repo
2. *cd* into either **singlevm** or **threevm** folder
3. run ```vagrant up```

## Dev/Test Environment

1. Windows 10 Pro x64 Version 1607 (OS Build 14393.351)
2. Vagrant, 1.8.6
3. VirtualBox, 5.1.4

## Folder structure

.
├── LICENSE  
├── provision  
│   ├── roles  
│   │   ├── aspnet  
│   │   │   ├── files  
│   │   │   │   ├── Program.cs  
│   │   │   │   ├── project.json  
│   │   │   │   ├── Startup.cs  
│   │   │   │   ├── UsgsData.cs  
│   │   │   │   └── UsgsService.cs  
│   │   │   ├── meta  
│   │   │   │   └── main.yml  
│   │   │   └── tasks  
│   │   │       ├── 10_install_aspnet.yml  
│   │   │       └── main.yml  
│   │   ├── common  
│   │   │   └── tasks  
│   │   │       ├── 10_update_sudoer.yml  
│   │   │       ├── 20_update_aptcache.yml  
│   │   │       └── main.yml  
│   │   └── nginx  
│   │       ├── files  
│   │       │   ├── default.single  
│   │       │   └── default.three  
│   │       ├── meta  
│   │       │   └── main.yml  
│   │       └── tasks  
│   │           ├── 10_install_nginx.yml  
│   │           └── main.yml  
│   ├── single_deploy.yml  
│   └── three_deploy.yml  
├── README.md  
├── singlevm  
│   └── Vagrantfile  
└── threevm  
    ├── testlb.sh  
    └── Vagrantfile  

## Dev Notes

1. Vagrant Ansible local provider installs Ansible into vm automatically, there is no such steps in above files.
2. In **singlevm** we have a shell provisioner that will check on port 80, and will fail the provision if nginx is not listening on 80.
3. Regarding updating file */etc/sudoers*:
   * User *vagrant* is set passwordless in file */etc/sudoers.d/10_vagrant* out of box,
   * Group *admin* is legacy and was allowed sudo by password by default to allow backwards compatibility,
   * Therefore our code here is basically no-op.
4. Re-run *vagrant provision* will not restart nginx, we used *nginx -s reload* anyway.
5. The web app is a very simple asp.net core app that will call USGS for recent Earth Quake data.
6. As my understanding the head node is both an nginx role and aspnet role, this can be changed however very easily.
7. In **threevm** we no longer test port 80, but fire a testing script to check on the return of each load-balanced aspnet host.

## Future improvements/Current compromises

1. Some people might combine the vagrant files into one, to handle single vm and multiple vms in one go. Yet We felt that's over-optimisation. Although it reduced one vagrant file, but it makes people trying to understand the single vm scenario more difficult. In the current design, each scenario locates in a single folder with one vagrant file and 0 to 1 testing script, all the provisioning code are shared between them.

2. We hard code 3 vms for **threevm**, this certainly can be improved to take arbitrary numbers. Yet again the requirement is not there just, so we took the fast route.

3. In **threevm** We were trying to limit the vm's memory, yet We found out that Vagrant is not generic on this (i.e. it demands extra logic/code to specify that to a certain provider, in our case it's VirtualBox). It will make our code tied to VirtualBox. We believe one can write better code to handle all providers, yet due to time limitation and easy access to all these providers, we choose to ignore the issue all together.

4. The understanding of apt-cache-update task can/should be improved, as currently it will always run if we *vagrant provision* again, not ideal. We considered to use *cache_valid_time* but both *aspnet install* and *nginx install* need this task, so we haven't figured out the clean solution on this.

5. Nginx conf file can be templated, this is related to issue 2.

6. We used **synced folder**, this will generate an empty *provision* folder on Windows, which is annoying.
