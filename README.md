# Counter Strike 2 Server Picker Plus
<div align="center">

  <a href="https://github.com/mschessler/cs2-server-picker-plus/releases"><img src="https://img.shields.io/github/downloads/mschessler/cs2-server-picker-plus/total.svg"/></a>
  <img src="https://img.shields.io/github/license/mschessler/cs2-server-picker-plus"/>
  <img src="https://img.shields.io/github/v/release/mschessler/cs2-server-picker-plus"/>
  <img src="https://img.shields.io/github/stars/mschessler/cs2-server-picker-plus"/>

</div>

A lightweight server picker for CS2 and Deadlock. Based on the original [CS2 Server Picker](https://github.com/FN-FAL113/cs2-server-picker) by FN-FAL113.

## ⬇️ Download
### [Releases](https://github.com/mschessler/cs2-server-picker-plus/releases)

## 📷 Screenshot
![image](https://github.com/user-attachments/assets/6f16fbc2-2b70-478f-8085-36c0e684fbe1)


## ⚙️ Requirements
- Windows 10 or above
- Future Linux support possible

## ❔FAQ
**1. How it works, will I get banned?!**
 - The app does not modify any game or system files, you are safe from being banned when using the app as long as you do not download from untrusted sources. It will add necessary firewall policies to block game server relay ip addresses from being accessed by your network thus skipping them in-game when finding a match.

**2. Not being routed to lowest ping server or not working on your location?**
  - Due to the fact that we can only access and block **_IP relay addresses_** from valve's network points around the world rather than the game's actual server IP addresses directly, which are **_not exposed_** publicly, either your connection got relayed to the nearest available server due to **_how Steam Datagram relay works_** or **_your location might be a factor_**. 
- Re-routing can also happen anytime, even mid-game. One of the best ways to test it out is to block low-ping servers and leave out high-ping servers that are far from your current region. If your ping is high in-game, then you are being routed properly, and the blocked IP relays are not able to re-route you to a nearby server. I was able to test this out properly way back.
- Some solutions that might help out but are not guaranteed: turning off any vpn, uninstalling third-party antivirus and let Windows Defender manage the firewall.
- ISP-related issues, such as bad routing or high ping, are out of scope and control since the app only adds firewall entries. Please contact your ISP instead.

**3. Why are admin permission required?<br>**
  - This is due to how Windows requires elevated execution when adding the necessary firewall policies. If the app is running in normal mode, it will not be able to do its operations and will throw errors.

**4. I'm receiving frequent timeouts when a match is being confirmed<br>**
  - You may have blocked many servers, for optimal searching and relaying block only the necessary server relays.

**5. Why windows only?<br>**
  - Linux support is possible and will be added in the future.

**6. Will this work for Deadlock?**
  - Yes. CS2 and Deadlock servers utilize the same server relay addresses.

## 🔽 Disclaimer
- This project is not affiliated, associated, authorized, endorsed by valve, its affiliates or subsidiaries. Images, names and other form of trademark are registered to their respective owners.
