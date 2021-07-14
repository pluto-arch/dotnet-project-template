/* eslint-disable */

import { get, post } from "../utils/request";

export const getToken = p => get("/account/getToken", p);

export const getLoginedUser = p => get("/account");
