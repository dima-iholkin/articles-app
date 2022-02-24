import React from 'react';
import { useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { MdMenu } from "react-icons/md";
import { Menu, MenuItem } from '@material-ui/core';
import { userManager } from "src/IdentityService/UserManager";
import { UserRolesEnum } from "src/IdentityService/UserRolesEnum";
import { setIsLogoutRedirect } from "src/Store/reducers/uiSlice";
import { store, storeType } from "src/Store/store";
import { useUserRole } from '../../../../../_Helpers/useUserRole';
import { Routes } from '../../../../Routing/Routes';



export function MenuButton() {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleMenuIconClick = (event: React.MouseEvent<HTMLDivElement>) => {
    setAnchorEl(event.currentTarget);
  }

  const handleCloseMenuAction = () => {
    setAnchorEl(null);
  }
  // Show and hide the menu logic.

  const history = useHistory();
  const notifications = useSelector((state: storeType) => state.notifications.notifications.entities);
  const userRole: UserRolesEnum[] = useUserRole();
  // Fetch the data from the store.



  const handleLoginClick = () => {
    userManager.signinRedirect({
      state: {
        prevPage: window.location.pathname
      }
    });
    handleCloseMenuAction();
  }

  const handleLogoutClick = () => {
    store.dispatch(setIsLogoutRedirect() as any);
    // Fix to not show the LoginPrompt for some second before the redirect to the Identity pages.

    userManager.signoutRedirect({
      state: {
        prevPage: window.location.pathname
      }
    });
    handleCloseMenuAction();
  }
  // Login and Logout logic.

  function handleGoToLocalPageClick(route: string) {
    return () => {
      history.push(route);
      handleCloseMenuAction();
    }
  }
  // Click to another page logic.

  function handleGoToExternalPageClick(route: string) {
    return () => {
      window.location.assign(route);
      handleCloseMenuAction();
    }
  }



  function generateMenuItemsList(): JSX.Element[] {
    const menuItems: JSX.Element[] = [];

    if (
      userRole === undefined
      || userRole.length === 0
      || userRole.includes(UserRolesEnum.Unauthenticated)
    ) {
      menuItems.push(
        <MenuItem key={3} onClick={handleLoginClick}>
          Login
        </MenuItem>,
        <MenuItem key={4} onClick={handleGoToExternalPageClick(Routes.identityRegister)}>
          Register
        </MenuItem>
      );

      return menuItems;
    }

    if (userRole.includes(UserRolesEnum.Moderator)) {
      const menuItemsGenerated = [];
      menuItemsGenerated.push(
        generateMenuItemWithRouteChange(Routes.articles.pending.root, "Pending Decisions"),
        generateMenuItemWithRouteChange(Routes.articles.my.decisions, "My Decisions"),
      );

      menuItemsGenerated.forEach(mi => {
        if (mi !== undefined) {
          menuItems.push(mi);
        }
      });
    }

    let menuItemsGenerated2 = [];
    menuItemsGenerated2.push(...generateCommonMenuItemsForAuthenticatedUsers());

    menuItemsGenerated2.forEach(mi => {
      if (mi !== undefined) {
        menuItems.push(mi);
      }
    });

    return menuItems;
  }



  function generateCommonMenuItemsForAuthenticatedUsers() {
    const newNotifsCount = notifications.filter(n => n.readAt_DateUtc === null).length;
    const menuItems = [
      generateMenuItemWithRouteChange(Routes.root, "All Articles"),
      generateMenuItemWithRouteChange(Routes.articles.my.root, "My Articles"),
      generateMenuItemWithRouteChange(Routes.notifications, `Notifications(${newNotifsCount})`),
      <MenuItem key={1} onClick={handleGoToExternalPageClick(Routes.identityAccountManage)} >
        Profile
      </MenuItem>,
      <MenuItem key={2} onClick={handleLogoutClick}>
        Logout
      </MenuItem>
    ];

    return menuItems;
  }

  function generateMenuItemWithRouteChange(route: string, label: string) {
    if (history.location.pathname === route) {
      return undefined;
    }

    return (
      <MenuItem key={route} onClick={handleGoToLocalPageClick(route)}>
        {label}
      </MenuItem>
    );
  }
  // Generate the menu items.



  let menuItems: JSX.Element[] = generateMenuItemsList();

  return (
    <div>
      <div
        aria-controls="simple-menu"
        aria-haspopup="true"
        onClick={handleMenuIconClick}
      >
        <MdMenu size={iconSize} />
      </div>
      <Menu
        id="simple-menu"
        anchorEl={anchorEl}
        keepMounted
        open={Boolean(anchorEl)}
        onClose={handleCloseMenuAction}
      >
        {menuItems}
      </Menu>
    </div>
  )
}



const iconSize: number = 40;