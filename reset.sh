#!/usr/bin/env bash
docker stop ids4admin
docker rm ids4admin
docker pull registry.cn-shanghai.aliyuncs.com/wsr/ids4admin:latest
docker run -d --name ids4admin --restart always -v /ids4admin:/ids4admin -p 6566:6566 registry.cn-shanghai.aliyuncs.com/wsr/ids4admin /ids4admin/appsettings.json