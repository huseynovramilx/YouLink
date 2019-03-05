SELECT * FROM AspnetUsers as u
JOIN RecipientTypes as rt
ON u.RecipientTypeID = rt.ID;